namespace TheAGEnt.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CheckGrade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Grades", "Graded", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Grades", "Graded");
        }
    }
}
