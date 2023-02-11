namespace MuseLab7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class creators : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Creators",
                c => new
                    {
                        CreatorID = c.Int(nullable: false, identity: true),
                        CreatorName = c.String(),
                        CreatorBio = c.String(),
                    })
                .PrimaryKey(t => t.CreatorID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Creators");
        }
    }
}
