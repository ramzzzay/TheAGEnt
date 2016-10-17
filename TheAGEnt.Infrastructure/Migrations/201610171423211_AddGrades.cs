namespace TheAGEnt.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGrades : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Picture_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pictures", t => t.Picture_Id)
                .Index(t => t.Picture_Id);
            
            CreateTable(
                "dbo.GradeUsers",
                c => new
                    {
                        Grade_Id = c.Int(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Grade_Id, t.User_Id })
                .ForeignKey("dbo.Grades", t => t.Grade_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Grade_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GradeUsers", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GradeUsers", "Grade_Id", "dbo.Grades");
            DropForeignKey("dbo.Grades", "Picture_Id", "dbo.Pictures");
            DropIndex("dbo.GradeUsers", new[] { "User_Id" });
            DropIndex("dbo.GradeUsers", new[] { "Grade_Id" });
            DropIndex("dbo.Grades", new[] { "Picture_Id" });
            DropTable("dbo.GradeUsers");
            DropTable("dbo.Grades");
        }
    }
}
