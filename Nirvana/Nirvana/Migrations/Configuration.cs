namespace Nirvana.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;
    using Nirvana.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Nirvana.Models.NirvanaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Nirvana.Models.NirvanaContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            // context.staticRanks.AddOrUpdate(\
            //      a => a.Title, 
            //     new staticRank { 
            // );

            var ExDefinitions = new List<RankDefinitions>
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

            ExDefinitions.ForEach(a => context.Definitions.Add(a));
            context.SaveChanges();
        }
    }
}
