namespace MuseLab7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class collabs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Collabs",
                c => new
                    {
                        CollabID = c.Int(nullable: false, identity: true),
                        CollabTitle = c.String(),
                        CollabDescription = c.String(),
                        IdeaID = c.Int(nullable: false),
                        CoCreatorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CollabID)
                .ForeignKey("dbo.CoCreators", t => t.CoCreatorID, cascadeDelete: true)
                .ForeignKey("dbo.Ideas", t => t.IdeaID, cascadeDelete: true)
                .Index(t => t.IdeaID)
                .Index(t => t.CoCreatorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Collabs", "IdeaID", "dbo.Ideas");
            DropForeignKey("dbo.Collabs", "CoCreatorID", "dbo.CoCreators");
            DropIndex("dbo.Collabs", new[] { "CoCreatorID" });
            DropIndex("dbo.Collabs", new[] { "IdeaID" });
            DropTable("dbo.Collabs");
        }
    }
}
