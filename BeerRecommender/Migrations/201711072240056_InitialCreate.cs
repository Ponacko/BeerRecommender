namespace BeerRecommender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Beers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Epm = c.String(),
                        AverageRating = c.Single(nullable: false),
                        Category = c.String(),
                        ImageUrl = c.String(),
                        Brewery_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Breweries", t => t.Brewery_Id)
                .Index(t => t.Brewery_Id);
            
            CreateTable(
                "dbo.Breweries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        City = c.String(),
                        Address = c.String(),
                        YearOfFoundation = c.Int(nullable: false),
                        ImageUrl = c.String(),
                        AverageRating = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRatings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rating = c.Single(nullable: false),
                        IsPrediction = c.Boolean(nullable: false),
                        Brewery_Id = c.Int(),
                        User_Id = c.Int(),
                        Beer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Breweries", t => t.Brewery_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.Beers", t => t.Beer_Id)
                .Index(t => t.Brewery_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Beer_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Age = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRatings", "Beer_Id", "dbo.Beers");
            DropForeignKey("dbo.Beers", "Brewery_Id", "dbo.Breweries");
            DropForeignKey("dbo.UserRatings", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserRatings", "Brewery_Id", "dbo.Breweries");
            DropIndex("dbo.UserRatings", new[] { "Beer_Id" });
            DropIndex("dbo.UserRatings", new[] { "User_Id" });
            DropIndex("dbo.UserRatings", new[] { "Brewery_Id" });
            DropIndex("dbo.Beers", new[] { "Brewery_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.UserRatings");
            DropTable("dbo.Breweries");
            DropTable("dbo.Beers");
        }
    }
}
