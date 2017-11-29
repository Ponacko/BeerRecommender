namespace BeerRecommender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class region_abbreviations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Regions", "Abbreviation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Regions", "Abbreviation");
        }
    }
}
