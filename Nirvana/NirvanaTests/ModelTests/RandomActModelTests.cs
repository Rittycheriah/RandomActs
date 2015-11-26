using System;
using System.Collections.Generic;
using Nirvana.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NirvanaTests.ModelTests
{
    [TestClass]
    public class RandomActModelTests
    {
        [TestMethod]
        public void RandomActEnsureICanCreateAnInstance()
        {
            // Arrange & Act
            RandomActsModel RandomAct = new RandomActsModel();

            // Assert
            Assert.IsNotNull(RandomAct);
        }

        [TestMethod]
        public void RandomActEnsurePropertiesWork()
        {
            // Arrange 
            RandomActsModel RandomAct = new RandomActsModel();
            List<Comment> list_comment = new List<Comment>();
            Comment this_comment = new Comment { UserComment = "hey" };
            list_comment.Add(this_comment);
            ApplicationUser this_user = new ApplicationUser();
            DateTime this_datetime = new DateTime(2015, 1, 16);
            
            // Act
            RandomAct.RandomActTitle = "Bought Coffee";
            RandomAct.RandomActDescription = "Bought Coffee for a girl who was having a bad day";
            RandomAct.PointsEarned = 3;
            RandomAct.Date = this_datetime;
            RandomAct.PicURL = "https://www.google.com/";
            RandomAct.Owner = this_user;
            RandomAct.Comments = list_comment;

            // Assert
            Assert.AreEqual("Bought Coffee", RandomAct.RandomActTitle);
            Assert.AreEqual(3, RandomAct.PointsEarned);
            Assert.AreEqual("Bought Coffee for a girl who was having a bad day", RandomAct.RandomActDescription);
            Assert.AreEqual(1, RandomAct.Comments.Count);
            Assert.AreEqual(this_datetime, RandomAct.Date);
            Assert.AreEqual("https://www.google.com/", RandomAct.PicURL);
            Assert.AreEqual(this_user, RandomAct.Owner);
        }
    }
}
