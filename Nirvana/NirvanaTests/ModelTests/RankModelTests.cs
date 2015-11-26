using System;
using Nirvana.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NirvanaTests.ModelTests
{
    [TestClass]
    public class RankModelTests
    {
        [TestMethod]
        public void RankEnsureICanCreateAnInstance()
        {
            Rank _rank = new Rank();
            Assert.IsNotNull(_rank);
        }

        [TestMethod]
        public void RankEnsurePropertiesWork()
        {
            Rank _rank = new Rank { Rank_Id = 1, BasePtsAllowance = 3, CommentFeat = false, MinimumPtReq = 10, SocialMedia = false, Name = "Grasshopper", Rank_Code = 1};

            Assert.AreEqual(1, _rank.Rank_Id);
            Assert.AreEqual(1, _rank.Rank_Code);
            Assert.AreEqual("Grasshopper", _rank.Name);
            Assert.AreEqual(false, _rank.CommentFeat);
            Assert.AreEqual(false, _rank.SocialMedia);
            Assert.AreEqual(3, _rank.BasePtsAllowance);
            Assert.AreEqual(10, _rank.MinimumPtReq);
        }
    }
}
