namespace BeerRecommender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class parse_source_change : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Breweries", "Region", c => c.String());
            AddColumn("dbo.Breweries", "Type", c => c.String());
            AddColumn("dbo.Breweries", "WebSiteUrl", c => c.String());
            DropColumn("dbo.Breweries", "AverageRating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Breweries", "AverageRating", c => c.Single(nullable: false));
            DropColumn("dbo.Breweries", "WebSiteUrl");
            DropColumn("dbo.Breweries", "Type");
            DropColumn("dbo.Breweries", "Region");
        }
    }
}
