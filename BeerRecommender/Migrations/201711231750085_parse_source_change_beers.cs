namespace BeerRecommender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class parse_source_change_beers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Beers", "AlcoholContentPercentage", c => c.Double(nullable: false));
            AddColumn("dbo.Beers", "Description", c => c.String());
            AlterColumn("dbo.Beers", "Epm", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Beers", "Epm", c => c.String());
            DropColumn("dbo.Beers", "Description");
            DropColumn("dbo.Beers", "AlcoholContentPercentage");
        }
    }
}
