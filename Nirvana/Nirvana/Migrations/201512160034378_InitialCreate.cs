namespace Nirvana.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RandomActsModels",
                c => new
                    {
                        RandomActId = c.Int(nullable: false, identity: true),
                        RandomActTitle = c.String(),
                        RandomActDescription = c.String(),
                        PointsEarned = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        PicURL = c.String(),
                        Owner_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.RandomActId)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id, cascadeDelete: true)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        UserComment = c.String(nullable: false, maxLength: 128),
                        CommentId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        ActId = c.Int(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                        RandomActsModel_RandomActId = c.Int(),
                    })
                .PrimaryKey(t => t.UserComment)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.RandomActsModels", t => t.RandomActsModel_RandomActId)
                .Index(t => t.User_Id)
                .Index(t => t.RandomActsModel_RandomActId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        LikeId = c.Int(nullable: false, identity: true),
                        Act_RandomActId = c.Int(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.LikeId)
                .ForeignKey("dbo.RandomActsModels", t => t.Act_RandomActId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Act_RandomActId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.RankDefinitions",
                c => new
                    {
                        RankingCode = c.Int(nullable: false, identity: true),
                        RankingName = c.String(),
                        RankingComments = c.Boolean(nullable: false),
                        RankingSocial = c.Boolean(nullable: false),
                        RankingBasePts = c.Int(nullable: false),
                        RankingMinPt = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RankingCode);
            
            CreateTable(
                "dbo.Ranks",
                c => new
                    {
                        Rank_Id = c.Int(nullable: false, identity: true),
                        Rank_Code = c.Int(nullable: false),
                        Name = c.String(),
                        CommentFeat = c.Boolean(nullable: false),
                        SocialMedia = c.Boolean(nullable: false),
                        BasePtsAllowance = c.Int(nullable: false),
                        MinimumPtReq = c.Int(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Rank_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Ranks", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.RandomActsModels", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Likes", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Likes", "Act_RandomActId", "dbo.RandomActsModels");
            DropForeignKey("dbo.Comments", "RandomActsModel_RandomActId", "dbo.RandomActsModels");
            DropForeignKey("dbo.Comments", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Ranks", new[] { "User_Id" });
            DropIndex("dbo.Likes", new[] { "User_Id" });
            DropIndex("dbo.Likes", new[] { "Act_RandomActId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Comments", new[] { "RandomActsModel_RandomActId" });
            DropIndex("dbo.Comments", new[] { "User_Id" });
            DropIndex("dbo.RandomActsModels", new[] { "Owner_Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Ranks");
            DropTable("dbo.RankDefinitions");
            DropTable("dbo.Likes");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Comments");
            DropTable("dbo.RandomActsModels");
        }
    }
}
