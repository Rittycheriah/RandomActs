using System;
using Nirvana.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NirvanaTests.ModelTests
{
    [TestClass]
    public class CommentModelTests
    {
        [TestMethod]
        public void CommentEnsureICanCreateAnInstance()
        {
            Comment Comment = new Comment();
            Assert.IsNotNull(Comment);
        }

        [TestMethod]
        public void CommentEnsurePropertiesWork()
        {
            Comment Comment = new Comment { UserComment = "I'm so glad you gave that girl five dollars" };

            ApplicationUser this_user = new ApplicationUser();
            Comment.User = this_user;

            Assert.AreEqual("I'm so glad you gave that girl five dollars", Comment.UserComment);
            Assert.AreEqual(Comment.User, this_user);
        }
    }
}
