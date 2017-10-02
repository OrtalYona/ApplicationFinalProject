namespace shauliTask3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class map : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Maps",
                c => new
                    {
                        MapsID = c.Int(nullable: false, identity: true),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.MapsID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Maps");
        }
    }
}
