namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusMovieTicket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MovieTickets", "Status", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MovieTickets", "Status");
        }
    }
}
