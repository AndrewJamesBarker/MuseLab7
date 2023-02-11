namespace MuseLab7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ideas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ideas",
                c => new
                    {
                        IdeaID = c.Int(nullable: false, identity: true),
                        IdeaTitle = c.String(),
                        IdeaDescription = c.String(),
                        CreatorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdeaID)
                .ForeignKey("dbo.Creators", t => t.CreatorID, cascadeDelete: true)
                .Index(t => t.CreatorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ideas", "CreatorID", "dbo.Creators");
            DropIndex("dbo.Ideas", new[] { "CreatorID" });
            DropTable("dbo.Ideas");
        }
    }
}
