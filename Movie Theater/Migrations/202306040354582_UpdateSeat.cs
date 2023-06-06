namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSeat : DbMigration
    {
        public override void Up()
        {

            DropColumn("dbo.Tickets", "Seat");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tickets", "Seat", c => c.String());

        }
    }
}
