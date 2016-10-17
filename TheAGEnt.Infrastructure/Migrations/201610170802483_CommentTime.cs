namespace TheAGEnt.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "PostingTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "PostingTime");
        }
    }
}
