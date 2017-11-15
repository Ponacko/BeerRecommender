namespace BeerRecommender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refactor_similarity : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User1_Id)
                .ForeignKey("dbo.Users", t => t.User2_Id)
                .Index(t => t.User1_Id)
                .Index(t => t.User2_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserSimilarities", "User2_Id", "dbo.Users");
            DropForeignKey("dbo.UserSimilarities", "User1_Id", "dbo.Users");
            DropIndex("dbo.UserSimilarities", new[] { "User2_Id" });
            DropIndex("dbo.UserSimilarities", new[] { "User1_Id" });
            DropTable("dbo.UserSimilarities");
        }
    }
}
