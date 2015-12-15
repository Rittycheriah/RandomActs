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
        private List<Likes> my_likes;
        private List<Comment> my_comments;
        private List<RandomActsModel> my_acts;
        private List<ApplicationUser> my_users;
        private Mock<DbSet<Comment>> mock_comment;
        private Mock<DbSet<Likes>> mock_likes;
        private Mock<DbSet<Rank>> mock_individual_rank;
        private Mock<DbSet<RankDefinitions>> mock_definitions;
        private Mock<DbSet<ApplicationUser>> mock_users;
        private List<Rank> my_rank = new List<Rank>();
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
            mock_comment = new Mock<DbSet<Comment>>();
            mock_likes = new Mock<DbSet<Likes>>();
            mock_individual_rank = new Mock<DbSet<Rank>>();
            mock_definitions = new Mock<DbSet<RankDefinitions>>();
            mock_users = new Mock<DbSet<ApplicationUser>>();
            my_acts = new List<RandomActsModel>();
            my_rank = new List<Rank>();
            my_likes = new List<Likes>();
            my_comments = new List<Comment>();
            my_users = new List<ApplicationUser>();
            owner = new ApplicationUser { Email = "a@a.com"};
            user1 = new ApplicationUser { Email = "b@b.com"};
            user2 = new ApplicationUser { Email = "c@c.com"};
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
        public void NirvanaRepoCanSumPoints()
        {
            //arrange
            my_acts.Add(new RandomActsModel { RandomActTitle = "Gave a donation", Owner = user1, PointsEarned = 3 });
            my_acts.Add(new RandomActsModel { RandomActTitle = "Gave someone a ride", Owner = user1, PointsEarned = 3 });
            my_acts.Add(new RandomActsModel { RandomActTitle = "Took care of someone's baby for a while", Owner = user1, PointsEarned = 3 });
            ConnectMocksToData();

            NirvanaRepository nirvana_repo = new NirvanaRepository(mock_context.Object);
            //act
            int result = nirvana_repo.GetTotalPoints(user1);

            // assert
            Assert.AreEqual(9, result);

        }

        [TestMethod]
        public void NirvanaRepoCanGetUserRank()
        {
            //arrange

            List<RankDefinitions> ExDefinitions = new List<RankDefinitions>
            {
                new RankDefinitions { RankingCode = 1, RankingBasePts = 3, RankingComments = false, RankingMinPt = 0, RankingName = "Grasshopper", RankingSocial = false},
                new RankDefinitions { RankingCode = 2, RankingBasePts = 3, RankingComments = true, RankingMinPt = 20, RankingName = "Student" , RankingSocial = false},
                new RankDefinitions { RankingCode = 3, RankingBasePts = 3, RankingComments = true, RankingMinPt = 30, RankingName = "Novice", RankingSocial = true},
                new RankDefinitions { RankingCode = 4, RankingBasePts = 6, RankingComments = true, RankingMinPt = 40, RankingName = "Apprentice", RankingSocial = true},
                new RankDefinitions { RankingCode = 5, RankingBasePts = 8, RankingComments = true, RankingMinPt = 60, RankingName = "Teacher", RankingSocial = true},
                new RankDefinitions { RankingCode = 6, RankingBasePts = 12, RankingComments = true, RankingMinPt = 75, RankingName = "Monk", RankingSocial = true},
                new RankDefinitions { RankingCode = 7, RankingBasePts = 15, RankingComments = true, RankingMinPt = 100, RankingName = "Elder", RankingSocial = true},
                new RankDefinitions { RankingCode = 8, RankingBasePts = 20, RankingComments = true, RankingMinPt = 150, RankingName = "Nirvana", RankingSocial = true}
            };

            var definitions = ExDefinitions.AsQueryable();

            mock_definitions.As<IQueryable<RankDefinitions>>().Setup(n => n.Provider).Returns(definitions.Provider);
            mock_definitions.As<IQueryable<RankDefinitions>>().Setup(n => n.GetEnumerator()).Returns(definitions.GetEnumerator());
            mock_definitions.As<IQueryable<RankDefinitions>>().Setup(n => n.ElementType).Returns(definitions.ElementType);
            mock_definitions.As<IQueryable<RankDefinitions>>().Setup(n => n.Expression).Returns(definitions.Expression);

            mock_context.Setup(v => v.Definitions).Returns(mock_definitions.Object);

            var current_rank = my_rank.AsQueryable();

            mock_individual_rank.As<IQueryable<Rank>>().Setup(n => n.Provider).Returns(current_rank.Provider);
            mock_individual_rank.As<IQueryable<Rank>>().Setup(n => n.GetEnumerator()).Returns(current_rank.GetEnumerator());
            mock_individual_rank.As<IQueryable<Rank>>().Setup(n => n.ElementType).Returns(current_rank.ElementType);
            mock_individual_rank.As<IQueryable<Rank>>().Setup(n => n.Expression).Returns(current_rank.Expression);

            mock_context.Setup(n => n.Ranks).Returns(mock_individual_rank.Object);

            my_acts.Add(new RandomActsModel { RandomActTitle = "Gave a donation", Owner = user1, PointsEarned = 3 });
            my_acts.Add(new RandomActsModel { RandomActTitle = "Gave someone a ride", Owner = user1, PointsEarned = 3 });
            ConnectMocksToData();

            NirvanaRepository nirvana_repo = new NirvanaRepository(mock_context.Object);
            
            // Act
            Rank user_rank = nirvana_repo.GetUserRank(user1);

            // Assert
            Assert.AreEqual(6, nirvana_repo.GetTotalPoints(user1));
            Assert.AreEqual("Grasshopper", user_rank.Name);           
        }

        [TestMethod]
        public void NirvanaRepoCanGetAllUsersRanks()
        {
            //arrange

            List<RankDefinitions> ExDefinitions = new List<RankDefinitions>
            {
                new RankDefinitions { RankingCode = 1, RankingBasePts = 3, RankingComments = false, RankingMinPt = 0, RankingName = "Grasshopper", RankingSocial = false},
                new RankDefinitions { RankingCode = 2, RankingBasePts = 3, RankingComments = true, RankingMinPt = 20, RankingName = "Student" , RankingSocial = false},
                new RankDefinitions { RankingCode = 3, RankingBasePts = 3, RankingComments = true, RankingMinPt = 30, RankingName = "Novice", RankingSocial = true},
                new RankDefinitions { RankingCode = 4, RankingBasePts = 6, RankingComments = true, RankingMinPt = 40, RankingName = "Apprentice", RankingSocial = true},
                new RankDefinitions { RankingCode = 5, RankingBasePts = 8, RankingComments = true, RankingMinPt = 60, RankingName = "Teacher", RankingSocial = true},
                new RankDefinitions { RankingCode = 6, RankingBasePts = 12, RankingComments = true, RankingMinPt = 75, RankingName = "Monk", RankingSocial = true},
                new RankDefinitions { RankingCode = 7, RankingBasePts = 15, RankingComments = true, RankingMinPt = 100, RankingName = "Elder", RankingSocial = true},
                new RankDefinitions { RankingCode = 8, RankingBasePts = 20, RankingComments = true, RankingMinPt = 150, RankingName = "Nirvana", RankingSocial = true}
            };

            var definitions = ExDefinitions.AsQueryable();

            mock_definitions.As<IQueryable<RankDefinitions>>().Setup(n => n.Provider).Returns(definitions.Provider);
            mock_definitions.As<IQueryable<RankDefinitions>>().Setup(n => n.GetEnumerator()).Returns(definitions.GetEnumerator());
            mock_definitions.As<IQueryable<RankDefinitions>>().Setup(n => n.ElementType).Returns(definitions.ElementType);
            mock_definitions.As<IQueryable<RankDefinitions>>().Setup(n => n.Expression).Returns(definitions.Expression);

            mock_context.Setup(v => v.Definitions).Returns(mock_definitions.Object);

            var current_rank = my_rank.AsQueryable();

            mock_individual_rank.As<IQueryable<Rank>>().Setup(n => n.Provider).Returns(current_rank.Provider);
            mock_individual_rank.As<IQueryable<Rank>>().Setup(n => n.GetEnumerator()).Returns(current_rank.GetEnumerator());
            mock_individual_rank.As<IQueryable<Rank>>().Setup(n => n.ElementType).Returns(current_rank.ElementType);
            mock_individual_rank.As<IQueryable<Rank>>().Setup(n => n.Expression).Returns(current_rank.Expression);

            mock_context.Setup(n => n.Ranks).Returns(mock_individual_rank.Object);

            my_acts.Add(new RandomActsModel { RandomActTitle = "Gave a donation", Owner = user1, PointsEarned = 6 });
            my_acts.Add(new RandomActsModel { RandomActTitle = "Gave someone a ride", Owner = user1, PointsEarned = 3 });
            my_acts.Add(new RandomActsModel { RandomActTitle = "Let someone take my place", Owner = user2, PointsEarned = 6 });
            my_acts.Add(new RandomActsModel { RandomActTitle = "c", Owner = owner, PointsEarned = 3});

            var all_users = my_users.AsQueryable();

            my_users.Add(owner);
            my_users.Add(user1);
            my_users.Add(user2);

            mock_users.As<IQueryable<ApplicationUser>>().Setup(n => n.Provider).Returns(all_users.Provider);
            mock_users.As<IQueryable<ApplicationUser>>().Setup(n => n.GetEnumerator()).Returns(all_users.GetEnumerator());
            mock_users.As<IQueryable<ApplicationUser>>().Setup(n => n.ElementType).Returns(all_users.ElementType);
            mock_users.As<IQueryable<ApplicationUser>>().Setup(n => n.Expression).Returns(all_users.Expression);

            mock_context.Setup(g => g.Users).Returns(mock_users.Object);
            ConnectMocksToData();

            NirvanaRepository nirvana_repo = new NirvanaRepository(mock_context.Object);

            // Act
            Dictionary<string, int> user_rank = nirvana_repo.GetAllUsersRanks();

            // Assert
            int recValue = 0;
            user_rank.TryGetValue(user1.Email, out recValue);
            Assert.AreEqual(9, recValue);
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
            // arrange
            NirvanaRepository nirvana_repo = new NirvanaRepository(mock_context.Object);
            Comment add_me = new Comment { ActId = 1, CommentId = 1, User = user1, UserComment = "yes" };
            my_acts.Add(new RandomActsModel { RandomActId = 1});
            ConnectMocksToData();

            //Act
            bool added_comment = nirvana_repo.CreateComment(add_me, 1);

            //Assert
            Assert.AreEqual(1, my_acts.First().Comments.Count);
            Assert.IsTrue(added_comment);
        }

        [TestMethod]
        public void NirvanaRepoCanDeleteComment()
        {
            // arrange
            NirvanaRepository nirvana_repo = new NirvanaRepository(mock_context.Object);
            Comment delete_me = new Comment { ActId = 1, CommentId = 1, User = user1, UserComment = "YAS" };
            my_acts.Add(new RandomActsModel { RandomActId = 1, Owner = owner, RandomActTitle = "saved a kitten" });
            ConnectMocksToData();

            bool AddedComment = nirvana_repo.CreateComment(delete_me, 1);

            // act
            bool deleted_comment = nirvana_repo.DeleteComment(1, 1);

            // Assert
            Assert.IsTrue(deleted_comment);
        }

        [TestMethod]
        public void NirvanaRepoCanUpdateComment()
        {
            // arrange
            ConnectMocksToData();

            my_acts.Add(new RandomActsModel { RandomActId = 1, RandomActTitle = "walked an old lady across the street" });

            var data = my_comments.AsQueryable();

            mock_comment.As<IQueryable<Comment>>().Setup(m => m.Provider).Returns(data.Provider);
            mock_comment.As<IQueryable<Comment>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mock_comment.As<IQueryable<Comment>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mock_comment.As<IQueryable<Comment>>().Setup(m => m.Expression).Returns(data.Expression);

            mock_context.Setup(m => m.Comments).Returns(mock_comment.Object);

            mock_comment.Setup(m => m.Add(It.IsAny<Comment>())).Callback((Comment c) => my_comments.Add(c));

            Comment comm_2Add = new Comment { ActId = 1, UserComment = "what up", CommentId = 1, User = user1 };

            NirvanaRepository nirvana_repo = new NirvanaRepository(mock_context.Object);

            my_comments.Add(comm_2Add);
           
            string the_change = "I think you walked my grandma dude.";

            // act
            bool result = nirvana_repo.UpdateComment(1, the_change);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NirvanaRepoCanGetLikeCountForAnAct()
        {
            // arrange
            mock_acts.Setup(b => b.Add(It.IsAny<RandomActsModel>())).Callback((RandomActsModel act) => my_acts.Add(act));
            mock_context.Setup(a => a.Acts).Returns(mock_acts.Object);

            mock_likes.Setup(e => e.Add(It.IsAny<Likes>())).Callback((Likes like) => my_likes.Add(like));
            mock_context.Setup(c => c.Likes).Returns(mock_likes.Object);
            ConnectMocksToData();

            RandomActsModel to_like = new RandomActsModel { RandomActTitle = "Gave donation", Owner = user2, RandomActId = 1, PointsEarned = 3 };
            my_acts.Add(to_like);
            NirvanaRepository nirvana_repo = new NirvanaRepository(mock_context.Object);

            Likes created_like = nirvana_repo.CreateLike(to_like, user1);
            Likes created_like2 = nirvana_repo.CreateLike(to_like, owner);

            var like_data = my_likes.AsQueryable();

            mock_likes.As<IQueryable<Likes>>().Setup(m => m.Provider).Returns(like_data.Provider);
            mock_likes.As<IQueryable<Likes>>().Setup(m => m.GetEnumerator()).Returns(like_data.GetEnumerator);
            mock_likes.As<IQueryable<Likes>>().Setup(m => m.ElementType).Returns(like_data.ElementType);
            mock_likes.As<IQueryable<Likes>>().Setup(m => m.Expression).Returns(like_data.Expression);

            mock_context.Setup(m => m.Likes).Returns(mock_likes.Object);


            // act
            int LikesResult = nirvana_repo.GetLikeCount(1);

            // assert
            Assert.AreEqual(2, LikesResult);
        }

        [TestMethod]
        public void NirvanaRepoCanCreateLike()
        {
            // arrange
            mock_acts.Setup(b => b.Add(It.IsAny<RandomActsModel>())).Callback((RandomActsModel act) => my_acts.Add(act));
            mock_context.Setup(a => a.Acts).Returns(mock_acts.Object);

            mock_likes.Setup(e => e.Add(It.IsAny<Likes>())).Callback((Likes like) => my_likes.Add(like));
            mock_context.Setup(c => c.Likes).Returns(mock_likes.Object);
            ConnectMocksToData();
            RandomActsModel to_like = new RandomActsModel { RandomActTitle = "Gave donation", Owner = user2, PointsEarned = 3 };
            my_acts.Add(to_like);
            NirvanaRepository nirvana_repo = new NirvanaRepository(mock_context.Object);

            // act
            Likes result = nirvana_repo.CreateLike(to_like, user1);

            //assert
            Assert.AreEqual(user1, result.User);
        }

        [TestMethod]
        public void NirvanaRepoCanDeleteLike()
        {
            // arrange
            NirvanaRepository nirvana_repo = new NirvanaRepository(mock_context.Object);
            my_likes.Add(new Likes { User = owner, LikeId = 1 });
            RandomActsModel to_like = new RandomActsModel { RandomActTitle = "Gave donation", Owner = user2, Likes = my_likes };

            ConnectMocksToData();

            var like_data = my_likes.AsQueryable();

            mock_likes.As<IQueryable<Likes>>().Setup(m => m.Provider).Returns(like_data.Provider);
            mock_likes.As<IQueryable<Likes>>().Setup(m => m.GetEnumerator()).Returns(like_data.GetEnumerator);
            mock_likes.As<IQueryable<Likes>>().Setup(m => m.ElementType).Returns(like_data.ElementType);
            mock_likes.As<IQueryable<Likes>>().Setup(m => m.Expression).Returns(like_data.Expression);

            mock_context.Setup(m => m.Likes).Returns(mock_likes.Object);

            // act
            bool deleted_like = nirvana_repo.DeleteLike(1);

            // Assert
            Assert.IsTrue(deleted_like);
        }

        [TestMethod]
        public void NirvanaRepoCanSearchOtherActs()
        {
            // arrange

            //act

            // assert
            throw new NotImplementedException();
        }

        [TestMethod]
        public void NirvanaRepoCanAddPointsToOtherUsers()
        {
            //arrange

            List<RankDefinitions> ExDefinitions = new List<RankDefinitions>
            {
                new RankDefinitions { RankingCode = 1, RankingBasePts = 3, RankingComments = false, RankingMinPt = 0, RankingName = "Grasshopper", RankingSocial = false},
                new RankDefinitions { RankingCode = 2, RankingBasePts = 3, RankingComments = true, RankingMinPt = 20, RankingName = "Student" , RankingSocial = false},
                new RankDefinitions { RankingCode = 3, RankingBasePts = 3, RankingComments = true, RankingMinPt = 30, RankingName = "Novice", RankingSocial = true},
                new RankDefinitions { RankingCode = 4, RankingBasePts = 6, RankingComments = true, RankingMinPt = 40, RankingName = "Apprentice", RankingSocial = true},
                new RankDefinitions { RankingCode = 5, RankingBasePts = 8, RankingComments = true, RankingMinPt = 60, RankingName = "Teacher", RankingSocial = true},
                new RankDefinitions { RankingCode = 6, RankingBasePts = 12, RankingComments = true, RankingMinPt = 75, RankingName = "Monk", RankingSocial = true},
                new RankDefinitions { RankingCode = 7, RankingBasePts = 15, RankingComments = true, RankingMinPt = 100, RankingName = "Elder", RankingSocial = true},
                new RankDefinitions { RankingCode = 8, RankingBasePts = 20, RankingComments = true, RankingMinPt = 150, RankingName = "Nirvana", RankingSocial = true}
            };

            var definitions = ExDefinitions.AsQueryable();

            mock_definitions.As<IQueryable<RankDefinitions>>().Setup(n => n.Provider).Returns(definitions.Provider);
            mock_definitions.As<IQueryable<RankDefinitions>>().Setup(n => n.GetEnumerator()).Returns(definitions.GetEnumerator());
            mock_definitions.As<IQueryable<RankDefinitions>>().Setup(n => n.ElementType).Returns(definitions.ElementType);
            mock_definitions.As<IQueryable<RankDefinitions>>().Setup(n => n.Expression).Returns(definitions.Expression);

            mock_context.Setup(v => v.Definitions).Returns(mock_definitions.Object);

            var current_rank = my_rank.AsQueryable();

            mock_individual_rank.As<IQueryable<Rank>>().Setup(n => n.Provider).Returns(current_rank.Provider);
            mock_individual_rank.As<IQueryable<Rank>>().Setup(n => n.GetEnumerator()).Returns(current_rank.GetEnumerator());
            mock_individual_rank.As<IQueryable<Rank>>().Setup(n => n.ElementType).Returns(current_rank.ElementType);
            mock_individual_rank.As<IQueryable<Rank>>().Setup(n => n.Expression).Returns(current_rank.Expression);

            mock_context.Setup(n => n.Ranks).Returns(mock_individual_rank.Object);

            mock_acts.Setup(b => b.Add(It.IsAny<RandomActsModel>())).Callback((RandomActsModel act) => my_acts.Add(act));
            mock_context.Setup(a => a.Acts).Returns(mock_acts.Object);

            mock_likes.Setup(e => e.Add(It.IsAny<Likes>())).Callback((Likes like) => my_likes.Add(like));
            mock_context.Setup(c => c.Likes).Returns(mock_likes.Object);

            ConnectMocksToData();
            RandomActsModel to_like = new RandomActsModel { RandomActTitle = "Gave donation", RandomActDescription = "gave a donation to St. Jude's", Owner = user2, Date = DateTime.Now};
            NirvanaRepository nirvana_repo = new NirvanaRepository(mock_context.Object);
            nirvana_repo.CreateAct(to_like.RandomActTitle, to_like.RandomActDescription, to_like.Date, to_like.Owner);

            //Act
            int totalpoints = nirvana_repo.AddLikePts(to_like, user2);

            // Assert
            Assert.AreEqual(6, totalpoints);
        }
    }
}
