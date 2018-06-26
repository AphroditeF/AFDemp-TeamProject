namespace AspNetExtendingIdentityRoles.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessageTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        DateSent = c.DateTime(nullable: false),
                        DeletedBySender = c.Boolean(nullable: false),
                        DeletedByReceiver = c.Boolean(nullable: false),
                        MessageBody = c.String(),
                        IsRead = c.Boolean(nullable: false),
                        ReceiverID = c.String(nullable: false, maxLength: 128),
                        SenderID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.AspNetUsers", t => t.ReceiverID)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderID)
                .Index(t => t.ReceiverID)
                .Index(t => t.SenderID);
            
            AlterColumn("dbo.AspNetRoles", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "SenderID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "ReceiverID", "dbo.AspNetUsers");
            DropIndex("dbo.Messages", new[] { "SenderID" });
            DropIndex("dbo.Messages", new[] { "ReceiverID" });
            AlterColumn("dbo.AspNetRoles", "Name", c => c.String());
            DropTable("dbo.Messages");
        }
    }
}
