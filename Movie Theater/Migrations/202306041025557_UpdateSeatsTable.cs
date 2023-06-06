namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSeatsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Seats", "IsAvailable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Seats", "IsAvailable");
        }
    }
}
