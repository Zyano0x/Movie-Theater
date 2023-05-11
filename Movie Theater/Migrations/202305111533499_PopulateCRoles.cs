namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateCRoles : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO CRoles (Name) VALUES (N'Actor')");
            Sql("INSERT INTO CRoles (Name) VALUES (N'Director')");
            Sql("INSERT INTO CRoles (Name) VALUES (N'Producer')");
            Sql("INSERT INTO CRoles (Name) VALUES (N'Writer')");
        }
        
        public override void Down()
        {
        }
    }
}
