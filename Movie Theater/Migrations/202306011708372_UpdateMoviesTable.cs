namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMoviesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "Url");
        }
    }
}
