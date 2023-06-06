namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCrewsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Crews", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Crews", "Url");
        }
    }
}
