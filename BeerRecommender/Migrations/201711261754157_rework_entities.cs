namespace BeerRecommender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rework_entities : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserRatings", "Beer_Id", "dbo.Beers");
            DropForeignKey("dbo.UserRatings", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserSimilarities", "User1_Id", "dbo.Users");
            DropForeignKey("dbo.UserSimilarities", "User2_Id", "dbo.Users");
            DropIndex("dbo.UserRatings", new[] { "Beer_Id" });
            DropIndex("dbo.UserRatings", new[] { "User_Id" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.UserSimilarities", new[] { "User1_Id" });
            DropIndex("dbo.UserSimilarities", new[] { "User2_Id" });
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TagBeers",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Beer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Beer_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Beers", t => t.Beer_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Beer_Id);
            
            AddColumn("dbo.Beers", "IsPopular", c => c.Boolean(nullable: false));
            AddColumn("dbo.Beers", "User_Id", c => c.Int());
            AddColumn("dbo.Beers", "User_Id1", c => c.Int());
            AddColumn("dbo.Breweries", "RegionString", c => c.String());
            AddColumn("dbo.Breweries", "Region_Id", c => c.Int());
            AddColumn("dbo.Users", "Region_Id", c => c.Int());
            AddColumn("dbo.Users", "Beer_Id", c => c.Int());
            AddColumn("dbo.Users", "Beer_Id1", c => c.Int());
            CreateIndex("dbo.Beers", "User_Id");
            CreateIndex("dbo.Beers", "User_Id1");
            CreateIndex("dbo.Breweries", "Region_Id");
            CreateIndex("dbo.Users", "Region_Id");
            CreateIndex("dbo.Users", "Beer_Id");
            CreateIndex("dbo.Users", "Beer_Id1");
            AddForeignKey("dbo.Breweries", "Region_Id", "dbo.Regions", "Id");
            AddForeignKey("dbo.Beers", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Beers", "User_Id1", "dbo.Users", "Id");
            AddForeignKey("dbo.Users", "Region_Id", "dbo.Regions", "Id");
            AddForeignKey("dbo.Users", "Beer_Id", "dbo.Beers", "Id");
            AddForeignKey("dbo.Users", "Beer_Id1", "dbo.Beers", "Id");
            DropColumn("dbo.Beers", "AverageRating");
            DropColumn("dbo.Breweries", "Region");
            DropColumn("dbo.Users", "UserName");
            DropColumn("dbo.Users", "Email");
            DropColumn("dbo.Users", "Age");
            DropTable("dbo.UserRatings");
            DropTable("dbo.UserSimilarities");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserSimilarities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Similarity = c.Double(nullable: false),
                        User1_Id = c.Int(),
                        User2_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRatings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rating = c.Single(nullable: false),
                        IsPrediction = c.Boolean(nullable: false),
                        Beer_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Users", "UserName", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Breweries", "Region", c => c.String());
            AddColumn("dbo.Beers", "AverageRating", c => c.Single(nullable: false));
            DropForeignKey("dbo.TagBeers", "Beer_Id", "dbo.Beers");
            DropForeignKey("dbo.TagBeers", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.Users", "Beer_Id1", "dbo.Beers");
            DropForeignKey("dbo.Users", "Beer_Id", "dbo.Beers");
            DropForeignKey("dbo.Users", "Region_Id", "dbo.Regions");
            DropForeignKey("dbo.Beers", "User_Id1", "dbo.Users");
            DropForeignKey("dbo.Beers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Breweries", "Region_Id", "dbo.Regions");
            DropIndex("dbo.TagBeers", new[] { "Beer_Id" });
            DropIndex("dbo.TagBeers", new[] { "Tag_Id" });
            DropIndex("dbo.Users", new[] { "Beer_Id1" });
            DropIndex("dbo.Users", new[] { "Beer_Id" });
            DropIndex("dbo.Users", new[] { "Region_Id" });
            DropIndex("dbo.Breweries", new[] { "Region_Id" });
            DropIndex("dbo.Beers", new[] { "User_Id1" });
            DropIndex("dbo.Beers", new[] { "User_Id" });
            DropColumn("dbo.Users", "Beer_Id1");
            DropColumn("dbo.Users", "Beer_Id");
            DropColumn("dbo.Users", "Region_Id");
            DropColumn("dbo.Breweries", "Region_Id");
            DropColumn("dbo.Breweries", "RegionString");
            DropColumn("dbo.Beers", "User_Id1");
            DropColumn("dbo.Beers", "User_Id");
            DropColumn("dbo.Beers", "IsPopular");
            DropTable("dbo.TagBeers");
            DropTable("dbo.Tags");
            DropTable("dbo.Regions");
            CreateIndex("dbo.UserSimilarities", "User2_Id");
            CreateIndex("dbo.UserSimilarities", "User1_Id");
            CreateIndex("dbo.Users", "Email", unique: true);
            CreateIndex("dbo.UserRatings", "User_Id");
            CreateIndex("dbo.UserRatings", "Beer_Id");
            AddForeignKey("dbo.UserSimilarities", "User2_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.UserSimilarities", "User1_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.UserRatings", "User_Id", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserRatings", "Beer_Id", "dbo.Beers", "Id", cascadeDelete: true);
        }
    }
}
