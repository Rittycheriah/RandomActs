using System;
using Nirvana.Models;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NirvanaTests.ModelTests
{
    [TestClass]
    public class NirvanaRepositoryTests
    {
        private Mock<NirvanaContext> mock_context;
        private Mock<DbSet<RandomActsModel>> mock_acts;
        private List<RandomActsModel> my_acts;
        private List<Comment> my_comments;
        private List<Likes> my_likes;
        private ApplicationUser owner, user1, user2;

        private void ConnectMocksToData()
        {
            var data = my_acts.AsQueryable();

            mock_acts.As<IQueryable<RandomActsModel>>().Setup(m => m.Provider).Returns(data.Provider);
            mock_acts.As<IQueryable<RandomActsModel>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);
            mock_acts.As<IQueryable<RandomActsModel>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mock_acts.As<IQueryable<RandomActsModel>>().Setup(m => m.Expression).Returns(data.Expression);

            mock_context.Setup(m => m.Acts).Returns(mock_acts.Object);
        }
        
        [TestInitialize]
        public void Initalize(){
            mock_context = new Mock<NirvanaContext>();
            mock_acts = new Mock<DbSet<RandomActsModel>>();
            my_acts = new List<RandomActsModel>();
            owner = new ApplicationUser();
            user1 = new ApplicationUser();
            user2 = new ApplicationUser();
        }

        [TestCleanup]
        public void Cleanup()
        {
            mock_context = null;
            mock_acts = null;
            my_acts = null;
        }

        [TestMethod]
        public void NirvanaRepositoryEnsureICanCreateAnInstance()
        {
            NirvanaRepository nirvana_repo = new NirvanaRepository(mock_context.Object);
            Assert.IsNotNull(nirvana_repo);
        }

        [TestMethod]
        public void NirvanaRepositoryEnsureICanGetAllActs()
        {
            // Arrange
            my_acts.Add(new RandomActsModel { RandomActTitle = "Gave a donation", Owner = user1 });
            my_acts.Add(new RandomActsModel { RandomActTitle = "Gave someone a ride", Owner = user2 });
            ConnectMocksToData();

            NirvanaRepository nirvana_repo = new NirvanaRepository(mock_context.Object);
       
            // Act
            List<RandomActsModel> ActsResult = nirvana_repo.GetAllActs();

            // Assert
            Assert.AreEqual(2, ActsResult.Count);
        }

        [TestMethod]
        public void NirvanaRepositoryEnsureICanGetAllActsForAUser( )
        {
            // Arrange
            my_acts.Add(new RandomActsModel { RandomActTitle = "donation", Owner = owner });
            my_acts.Add(new RandomActsModel { RandomActTitle = "puppy", Owner = owner });
            ConnectMocksToData();

            NirvanaRepository nirvana_repo = new NirvanaRepository(mock_context.Object);

            //Act
            List<RandomActsModel> ActsResult = nirvana_repo.GetAllActs(owner);

            //Assert
            Assert.AreEqual(2, ActsResult.Count);
            Assert.AreEqual("donation", ActsResult.First().RandomActTitle);
        }

        [TestMethod]
        public void NirvanaRepoCanCreateAct()
        {
            //Arrange
            ConnectMocksToData();
            mock_acts.Setup(b => b.Add(It.IsAny<RandomActsModel>())).Callback((RandomActsModel act) => my_acts.Add(act));
            mock_context.Setup(a => a.Acts).Returns(mock_acts.Object);

            NirvanaRepository nirvana_repo = new NirvanaRepository(mock_context.Object);
            string title = "Gave a friend a ride";
            string description = "My friend was walking home from school, but I decided since it was cold it was better for them to ride with me";
            DateTime date = new DateTime(2015, 01, 01);

            //Act
            RandomActsModel added_act = nirvana_repo.CreateAct(title, description, date, owner);

            //Assert
            Assert.IsNotNull(added_act);
            mock_acts.Verify(n => n.Add(It.IsAny<RandomActsModel>()));
            mock_context.Verify(c => c.SaveChanges(), Times.Once());
            // need to run method for ActCount
        }

        [TestMethod]
        public void NirvanaRepoCanCountTotalActs()
        {
            // arrange
            ConnectMocksToData();
            NirvanaRepository nirvana_repo = new NirvanaRepository(mock_context.Object);
            string title = "Gave a friend a ride";
            string description = "My friend was walking home from school, but I decided since it was cold it was better for them to ride with me";
            DateTime date = new DateTime(2015, 01, 01);
            RandomActsModel added_act = nirvana_repo.CreateAct(title, description, date, owner);
            my_acts.Add(added_act);

            // act
            int result = nirvana_repo.GetActCount();

            // assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void NirvanaRepoCanCount4OneUser()
        {
            // arrange
            ConnectMocksToData();
            NirvanaRepository nirvana_repo = new NirvanaRepository(mock_context.Object);
            string title = "Gave a friend a ride";
            string description = "My friend was walking home from school, but I decided since it was cold it was better for them to ride with me";
            DateTime date = new DateTime(2015, 01, 01);
            RandomActsModel added_act = nirvana_repo.CreateAct(title, description, date, owner);
            my_acts.Add(added_act);

            // act
            int result = nirvana_repo.GetActCount(owner);
            int result2 = nirvana_repo.GetActCount(user1);

            // assert
            Assert.AreEqual(1, result);
            Assert.AreEqual(0, result2);
        }

        [TestMethod]
        public void NirvanaRepoCanGetUserRank()
        {
            ConnectMocksToData();
            NirvanaRepository nirvana_repo = new NirvanaRepository(mock_context.Object);
            // not sure how to associate a rank with a user. Should user have a rank_id? How
            // do I modify the user itself? 
            Assert.Equals("Grasshopper", owner.Rank);
        }

        [TestMethod]
        public void NirvanaRepoCanGetAllComments()
        {
            // arrange
            var comment = new List<Comment>
            {
               new Comment { User = owner, UserComment = "a"}
            };

            my_acts.Add(new RandomActsModel { RandomActTitle = "Gave donation", Owner = user1, Comments = comment });
            my_acts.Add(new RandomActsModel { RandomActTitle = "Gave donation", Owner = user2, Comments = comment });
            ConnectMocksToData(); 
            NirvanaRepository nirvana_repo = new NirvanaRepository(mock_context.Object);

            // act
            List<Comment> result = nirvana_repo.GetAllComments();

            //assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void NirvanaRepoCanGetCommentByAct()
        {
            // arrange
            var comment = new List<Comment>
            {
               new Comment { User = owner, UserComment = "b"}
            };

            var comment2 = new List<Comment>
            {
                new Comment { User = user2, UserComment = "act"}
            };

            my_acts.Add(new RandomActsModel { RandomActId = 1, RandomActTitle = "puppy", Owner = user1, Comments = comment});
            my_acts.Add(new RandomActsModel { RandomActId = 2, RandomActTitle = "kitten saved", Owner = user2, Comments = comment });
            ConnectMocksToData();
            NirvanaRepository nirvana_repo = new NirvanaRepository(mock_context.Object);

            // act
            List<Comment> result = nirvana_repo.GetAllComments(1);

            //assert
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void NirvanaRepoCanCreateComment()
        {

        }
    }
}
