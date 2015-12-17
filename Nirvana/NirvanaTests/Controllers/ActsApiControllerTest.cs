using System;
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
        DateTime ActDate = new DateTime(2015, 12, 18, 6, 12, 1);

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
            inst_of_controller = new ActsController(fake_repo.Object);

            fake_repo.Setup(r => r.GetAllActs(user1)).
                Returns(list_of_acts.Where(a => a.Owner == user1).ToList());

            fake_repo.Setup(r => r.CreateAct(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<DateTime>(), It.IsAny<ApplicationUser>())).
                Returns(new RandomActsModel { RandomActTitle = ActTitle, RandomActDescription = ActDescription, Date = ActDate, Owner = user1});
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
            IEnumerable<RandomActsModel> actual;
            Assert.IsTrue(response.TryGetContentValue<IEnumerable<RandomActsModel>>(out actual));
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
            var response = inst_of_controller.Post(ActTitle, ActDescription, ActDate, user1);

            Assert.AreEqual(ActTitle, response.RandomActTitle);
        }
    }
}
