namespace BeerRecommender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inverse_property : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Beers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Beers", "User_Id1", "dbo.Users");
            DropForeignKey("dbo.Users", "Beer_Id", "dbo.Beers");
            DropForeignKey("dbo.Users", "Beer_Id1", "dbo.Beers");
            DropIndex("dbo.Beers", new[] { "User_Id" });
            DropIndex("dbo.Beers", new[] { "User_Id1" });
            DropIndex("dbo.Users", new[] { "Beer_Id" });
            DropIndex("dbo.Users", new[] { "Beer_Id1" });
            CreateTable(
                "dbo.BeerUsers",
                c => new
                    {
                        Beer_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Beer_Id, t.User_Id })
                .ForeignKey("dbo.Beers", t => t.Beer_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Beer_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.BeerUser1",
                c => new
                    {
                        Beer_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Beer_Id, t.User_Id })
                .ForeignKey("dbo.Beers", t => t.Beer_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Beer_Id)
                .Index(t => t.User_Id);
            
            DropColumn("dbo.Beers", "User_Id");
            DropColumn("dbo.Beers", "User_Id1");
            DropColumn("dbo.Users", "Beer_Id");
            DropColumn("dbo.Users", "Beer_Id1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Beer_Id1", c => c.Int());
            AddColumn("dbo.Users", "Beer_Id", c => c.Int());
            AddColumn("dbo.Beers", "User_Id1", c => c.Int());
            AddColumn("dbo.Beers", "User_Id", c => c.Int());
            DropForeignKey("dbo.BeerUser1", "User_Id", "dbo.Users");
            DropForeignKey("dbo.BeerUser1", "Beer_Id", "dbo.Beers");
            DropForeignKey("dbo.BeerUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.BeerUsers", "Beer_Id", "dbo.Beers");
            DropIndex("dbo.BeerUser1", new[] { "User_Id" });
            DropIndex("dbo.BeerUser1", new[] { "Beer_Id" });
            DropIndex("dbo.BeerUsers", new[] { "User_Id" });
            DropIndex("dbo.BeerUsers", new[] { "Beer_Id" });
            DropTable("dbo.BeerUser1");
            DropTable("dbo.BeerUsers");
            CreateIndex("dbo.Users", "Beer_Id1");
            CreateIndex("dbo.Users", "Beer_Id");
            CreateIndex("dbo.Beers", "User_Id1");
            CreateIndex("dbo.Beers", "User_Id");
            AddForeignKey("dbo.Users", "Beer_Id1", "dbo.Beers", "Id");
            AddForeignKey("dbo.Users", "Beer_Id", "dbo.Beers", "Id");
            AddForeignKey("dbo.Beers", "User_Id1", "dbo.Users", "Id");
            AddForeignKey("dbo.Beers", "User_Id", "dbo.Users", "Id");
        }
    }
}
