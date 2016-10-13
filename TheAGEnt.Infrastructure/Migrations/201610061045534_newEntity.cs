namespace TheAGEnt.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Discription = c.String(),
                        UserId_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId_Id)
                .Index(t => t.UserId_Id);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Label = c.String(),
                        Discription = c.String(),
                        PathToImage = c.String(),
                        AlbumId_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Albums", t => t.AlbumId_Id)
                .Index(t => t.AlbumId_Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        PictureId_Id = c.Int(),
                        UserId_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pictures", t => t.PictureId_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId_Id)
                .Index(t => t.PictureId_Id)
                .Index(t => t.UserId_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Albums", "UserId_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "UserId_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "PictureId_Id", "dbo.Pictures");
            DropForeignKey("dbo.Pictures", "AlbumId_Id", "dbo.Albums");
            DropIndex("dbo.Comments", new[] { "UserId_Id" });
            DropIndex("dbo.Comments", new[] { "PictureId_Id" });
            DropIndex("dbo.Pictures", new[] { "AlbumId_Id" });
            DropIndex("dbo.Albums", new[] { "UserId_Id" });
            DropTable("dbo.Comments");
            DropTable("dbo.Pictures");
            DropTable("dbo.Albums");
        }
    }
}
