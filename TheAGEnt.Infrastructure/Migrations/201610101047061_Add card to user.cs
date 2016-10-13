namespace TheAGEnt.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addcardtouser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PathToCard", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PathToCard");
        }
    }
}
