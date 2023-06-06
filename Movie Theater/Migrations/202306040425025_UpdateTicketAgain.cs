namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTicketAgain : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "Seat", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickets", "Seat");
        }
    }
}
