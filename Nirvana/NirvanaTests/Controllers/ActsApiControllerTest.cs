using System;
using System.Web.Mvc;
using Nirvana.Controllers;
using System.Web.Http.Results;
using System.Net.Http;
using System.Web.Http;
using System.Collections.Generic;
using Nirvana.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NirvanaTests.Controllers
{
    [TestClass]
    public class ActsApiControllerTest
    {
        [TestMethod]
        public void ActsApiEnsureICanCallGetMethod()
        {
            // arrange
            var act_controller = new ActsController();
            act_controller.Request = new HttpRequestMessage();
            act_controller.Configuration = new HttpConfiguration();

            // act
            var response = act_controller.GetAllActs();

            // assert
            RandomActsModel actual;
            Assert.IsTrue(response.TryGetContentValue<RandomActsModel>(out actual));
        }
    }
}
