namespace BeerRecommender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserName_length_limit : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "UserName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.Users", "UserName", unique: true);
            CreateIndex("dbo.Users", "Email", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Users", new[] { "UserName" });
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "UserName", c => c.String(nullable: false));
        }
    }
}
