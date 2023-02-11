namespace MuseLab7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cocreators : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CoCreators",
                c => new
                    {
                        CoCreatorID = c.Int(nullable: false, identity: true),
                        CoCreatorName = c.String(),
                        CoCreatorBio = c.String(),
                    })
                .PrimaryKey(t => t.CoCreatorID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CoCreators");
        }
    }
}
