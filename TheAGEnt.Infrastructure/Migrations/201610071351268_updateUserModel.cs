namespace TheAGEnt.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateUserModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            AddColumn("dbo.AspNetUsers", "PathToPhoto", c => c.String());
            DropColumn("dbo.AspNetUsers", "Adress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Adress", c => c.String());
            DropColumn("dbo.AspNetUsers", "PathToPhoto");
            DropColumn("dbo.AspNetUsers", "Address");
        }
    }
}
