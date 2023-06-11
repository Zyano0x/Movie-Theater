namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameShowingsTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Showings", newName: "Showtimes");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Showtimes", newName: "Showings");
        }
    }
}
