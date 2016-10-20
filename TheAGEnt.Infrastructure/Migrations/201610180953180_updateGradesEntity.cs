namespace TheAGEnt.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateGradesEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Grades", "NumberOfGrade", c => c.Int(nullable: false));
            DropColumn("dbo.Grades", "Number");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Grades", "Number", c => c.Int(nullable: false));
            DropColumn("dbo.Grades", "NumberOfGrade");
        }
    }
}
