namespace shauliTask3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class email : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "PostID", "dbo.Posts");
            DropIndex("dbo.Comments", new[] { "PostID" });
            CreateTable(
                "dbo.UsetAccounts",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        ComfirmPassword = c.String(),
                        IsAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
            DropTable("dbo.Comments");
            DropTable("dbo.Posts");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostID = c.Int(nullable: false, identity: true),
                        PostTitle = c.String(),
                        postWriter = c.String(),
                        postWebSiteLink = c.String(),
                        date = c.DateTime(nullable: false),
                        text = c.String(),
                        video = c.String(),
                        image = c.String(),
                        counter = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PostID);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        CommentTitle = c.String(),
                        CommentWriter = c.String(),
                        commentWebSiteLink = c.String(),
                        text = c.String(),
                        PostID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentID);
            
            DropTable("dbo.UsetAccounts");
            CreateIndex("dbo.Comments", "PostID");
            AddForeignKey("dbo.Comments", "PostID", "dbo.Posts", "PostID", cascadeDelete: true);
        }
    }
}
