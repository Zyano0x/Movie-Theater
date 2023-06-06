namespace Movie_Theater.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sliders",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Img = c.String(nullable: false),
                    Title = c.String(nullable: false),
                    Description = c.String(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Crews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        DateOfBirth = c.DateTime(nullable: false),
                        Birthplace = c.String(maxLength: 255),
                        Biography = c.String(),
                        AvatarPath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MovieCrews",
                c => new
                    {
                        MovieId = c.Int(nullable: false),
                        CrewId = c.Int(nullable: false),
                        CRoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MovieId, t.CrewId, t.CRoleId })
                .ForeignKey("dbo.Crews", t => t.CrewId, cascadeDelete: true)
                .ForeignKey("dbo.CRoles", t => t.CRoleId, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.MovieId, cascadeDelete: true)
                .Index(t => t.MovieId)
                .Index(t => t.CrewId)
                .Index(t => t.CRoleId);
            
            CreateTable(
                "dbo.CRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 255),
                        ReleaseDate = c.DateTime(nullable: false),
                        Synopsis = c.String(),
                        Rating = c.String(nullable: false),
                        Runtime = c.Int(nullable: false),
                        Score = c.Int(nullable: false),
                        TrailerUrl = c.String(nullable: false),
                        PosterPath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MovieGenres",
                c => new
                    {
                        MovieId = c.Int(nullable: false),
                        GenreId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MovieId, t.GenreId })
                .ForeignKey("dbo.Genres", t => t.GenreId, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.MovieId, cascadeDelete: true)
                .Index(t => t.MovieId)
                .Index(t => t.GenreId);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        MovieId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Scores = c.Int(nullable: false),
                        Comment = c.String(),
                        Time = c.DateTime(nullable: false),
                        IsChanged = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.MovieId, t.UserId })
                .ForeignKey("dbo.Movies", t => t.MovieId, cascadeDelete: true)
                .Index(t => t.MovieId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Reviews", "MovieId", "dbo.Movies");
            DropForeignKey("dbo.MovieGenres", "MovieId", "dbo.Movies");
            DropForeignKey("dbo.MovieGenres", "GenreId", "dbo.Genres");
            DropForeignKey("dbo.MovieCrews", "MovieId", "dbo.Movies");
            DropForeignKey("dbo.MovieCrews", "CRoleId", "dbo.CRoles");
            DropForeignKey("dbo.MovieCrews", "CrewId", "dbo.Crews");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Reviews", new[] { "MovieId" });
            DropIndex("dbo.MovieGenres", new[] { "GenreId" });
            DropIndex("dbo.MovieGenres", new[] { "MovieId" });
            DropIndex("dbo.MovieCrews", new[] { "CRoleId" });
            DropIndex("dbo.MovieCrews", new[] { "CrewId" });
            DropIndex("dbo.MovieCrews", new[] { "MovieId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.MovieTickets");
            DropTable("dbo.Reviews");
            DropTable("dbo.Genres");
            DropTable("dbo.MovieGenres");
            DropTable("dbo.Movies");
            DropTable("dbo.CRoles");
            DropTable("dbo.MovieCrews");
            DropTable("dbo.Crews");
            DropTable("dbo.Slider");
        }
    }
}
