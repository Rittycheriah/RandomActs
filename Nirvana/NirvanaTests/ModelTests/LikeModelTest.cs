using System;
using Nirvana.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NirvanaTests.ModelTests
{
    [TestClass]
    public class LikeModelTest
    {
        [TestMethod]
        public void LikeModelEnsureICanCreateAnInstance()
        {
            Likes _like = new Likes();
            Assert.IsNotNull(_like);
        }

        [TestMethod]
        public void LikeModelEnsurePropertiesWork()
        {
            RandomActsModel act = new RandomActsModel { RandomActId = 1};
            Likes _like = new Likes();
            _like.Liked = true;
            _like.ActId = act.RandomActId;

            Assert.AreEqual(true, _like.Liked);
            Assert.AreEqual(1, _like.ActId);
        }
    }
}
