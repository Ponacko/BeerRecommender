namespace BeerRecommender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_user_similarity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserRatings", "Brewery_Id", "dbo.Breweries");
            DropForeignKey("dbo.UserRatings", "Beer_Id", "dbo.Beers");
            DropForeignKey("dbo.UserRatings", "User_Id", "dbo.Users");
            DropIndex("dbo.UserRatings", new[] { "Brewery_Id" });
            DropIndex("dbo.UserRatings", new[] { "User_Id" });
            DropIndex("dbo.UserRatings", new[] { "Beer_Id" });
            AlterColumn("dbo.UserRatings", "User_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.UserRatings", "Beer_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.UserRatings", "Beer_Id");
            CreateIndex("dbo.UserRatings", "User_Id");
            AddForeignKey("dbo.UserRatings", "Beer_Id", "dbo.Beers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserRatings", "User_Id", "dbo.Users", "Id", cascadeDelete: true);
            DropColumn("dbo.UserRatings", "Brewery_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserRatings", "Brewery_Id", c => c.Int());
            DropForeignKey("dbo.UserRatings", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserRatings", "Beer_Id", "dbo.Beers");
            DropIndex("dbo.UserRatings", new[] { "User_Id" });
            DropIndex("dbo.UserRatings", new[] { "Beer_Id" });
            AlterColumn("dbo.UserRatings", "Beer_Id", c => c.Int());
            AlterColumn("dbo.UserRatings", "User_Id", c => c.Int());
            CreateIndex("dbo.UserRatings", "Beer_Id");
            CreateIndex("dbo.UserRatings", "User_Id");
            CreateIndex("dbo.UserRatings", "Brewery_Id");
            AddForeignKey("dbo.UserRatings", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.UserRatings", "Beer_Id", "dbo.Beers", "Id");
            AddForeignKey("dbo.UserRatings", "Brewery_Id", "dbo.Breweries", "Id");
        }
    }
}
