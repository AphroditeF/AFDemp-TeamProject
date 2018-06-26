namespace AspNetExtendingIdentityRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mess : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "UserName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "UserName", c => c.String());
        }
    }
}
