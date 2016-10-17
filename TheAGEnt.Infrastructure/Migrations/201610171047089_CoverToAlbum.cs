namespace TheAGEnt.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CoverToAlbum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Albums", "PathToCover", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Albums", "PathToCover");
        }
    }
}
