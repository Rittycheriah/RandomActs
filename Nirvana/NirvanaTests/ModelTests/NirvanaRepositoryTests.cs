﻿using System;
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
    }
}
