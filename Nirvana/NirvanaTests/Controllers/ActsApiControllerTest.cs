﻿using System;
using System.Web.Mvc;
using Nirvana.Controllers;
using System.Web.Http.Results;
using System.Net.Http;
using System.Web.Http;
using System.Collections.Generic;
using Nirvana.Models;
using Moq;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NirvanaTests.Controllers
{
    [TestClass]
    public class ActsApiControllerTest
    {
        private static ApplicationUser user1 = new ApplicationUser();
        private static ApplicationUser user2 = new ApplicationUser();
        string ActTitle = "Gave someone a compliment";
        string ActDescription = "This girl was wearing a really cute dress, and I told her it was awesome";

        public INirvanaRepository int_repo;
        private ActsController inst_of_controller;

        public List<RandomActsModel> list_of_acts = new List<RandomActsModel>
        {
            new RandomActsModel { RandomActId = 1, RandomActTitle = "puppy", Owner = user1 },
            new RandomActsModel { RandomActId = 2, RandomActTitle = "kitten saved", Owner = user2 },
            new RandomActsModel { RandomActId = 3, RandomActTitle = "raccoon has home", Owner = user1}
        };

        [TestInitialize]
        public void Initalize()
        {
            var fake_repo = new Mock<INirvanaRepository>();
            fake_repo.Setup(r => r.GetAllActs()).Returns(list_of_acts);
            //inst_of_controller = new ActsController(fake_repo.Object);

            fake_repo.Setup(r => r.GetAllActs(user1)).
                Returns(list_of_acts.Where(a => a.Owner == user1).ToList());

            fake_repo.Setup(r => r.CreateAct(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<ApplicationUser>())).
                Returns(new RandomActsModel { RandomActTitle = ActTitle, RandomActDescription = ActDescription, Owner = user1, PointsEarned = 3});
        }

        [TestCleanup]
        public void Cleanup()
        {
            inst_of_controller = null;
        }

        [TestMethod]
        public void ActsApiEnsureICanCallGetMethod()
        {

            // arrange
            inst_of_controller.Request = new HttpRequestMessage();
            inst_of_controller.Configuration = new HttpConfiguration();

            // act
            var response = inst_of_controller.GetAllActs();

            // assert
            Assert.AreEqual(list_of_acts[0].ToString(), response.ToList().First().ToString());
        }

        [TestMethod]
        public void ActsApiEnsureICanCallGetUsersActs()
        {
            inst_of_controller.Request = new HttpRequestMessage();
            inst_of_controller.Configuration = new HttpConfiguration();

            var response = inst_of_controller.GetUserAct(user1);

            IEnumerable<RandomActsModel> actual;
            Assert.IsTrue(response.TryGetContentValue<IEnumerable<RandomActsModel>>(out actual));
        }

        [TestMethod]
        public void ActsApiEnsureICanPostNewAct()
        {
            //inst_of_controller.Configuration = new HttpConfiguration();
            RandomActsModel noob = new RandomActsModel { RandomActTitle = "winning", RandomActDescription = "always", Owner = user1 };
            var response = inst_of_controller.Post(noob);

            Assert.AreEqual(ActTitle, response.RandomActTitle);
        }

        [TestMethod]
        public void ActsApiEnsureCreatingActUsesPtsSystem()
        {

            var response = inst_of_controller.Post(list_of_acts[0]);

            Assert.AreEqual(3, response.PointsEarned);
        }
    }
}
