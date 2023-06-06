namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changess : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Seats", "OrderId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Seats", "OrderId");
        }
    }
}
