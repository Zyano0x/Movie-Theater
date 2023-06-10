namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateGenres : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Genres (Name) VALUES (N'Hành Động')");
            Sql("INSERT INTO Genres (Name) VALUES (N'Tài Liệu')");
            Sql("INSERT INTO Genres (Name) VALUES (N'Kinh Dị')");
            Sql("INSERT INTO Genres (Name) VALUES (N'Giật Gân')");
            Sql("INSERT INTO Genres (Name) VALUES (N'Drama')");
            Sql("INSERT INTO Genres (Name) VALUES (N'Chiến Tranh')");
            Sql("INSERT INTO Genres (Name) VALUES (N'Khoa Học - Viễn Tưởng')");
            Sql("INSERT INTO Genres (Name) VALUES (N'Phiêu Lưu')");
            Sql("INSERT INTO Genres (Name) VALUES (N'Huyền Bí')");
            Sql("INSERT INTO Genres (Name) VALUES (N'Anime')");
        }
        
        public override void Down()
        {
        }
    }
}
