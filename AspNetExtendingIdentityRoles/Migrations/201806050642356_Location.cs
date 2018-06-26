namespace AspNetExtendingIdentityRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Location : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "Location", c => c.String());
            DropColumn("dbo.Messages", "Coords");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "Coords", c => c.String());
            DropColumn("dbo.Messages", "Location");
        }
    }
}
