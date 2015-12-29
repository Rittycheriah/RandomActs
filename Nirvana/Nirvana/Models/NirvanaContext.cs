using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Nirvana.Models
{
    public class NirvanaContext : ApplicationDbContext
    {
        // Need to find the connection string for DB web.config
        public virtual DbSet<RandomActsModel> Acts { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Rank> Ranks { get; set; }
        public virtual DbSet<Likes> Likes { get; set; }
        public virtual DbSet<RankDefinitions> Definitions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

}