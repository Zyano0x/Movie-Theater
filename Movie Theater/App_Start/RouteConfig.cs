using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Movie_Theater
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "NewsArticle",
                url: "article/{name}",
                defaults: new { controller = "News", action = "Details", name = UrlParameter.Optional },
                namespaces: new[] { "Movie_Theater.Controllers" }
            );

            routes.MapRoute(
                name: "CrewDetails",
                url: "c/{name}",
                defaults: new { controller = "Crews", action = "Details", name = UrlParameter.Optional },
                namespaces: new[] { "Movie_Theater.Controllers" }
            );

            routes.MapRoute(
                name: "MovieDetails",
                url: "m/{name}",
                defaults: new { controller = "Movies", action = "Details", name = UrlParameter.Optional},
                namespaces: new[] { "Movie_Theater.Controllers" }
            );

            routes.MapRoute(
                name: "UserManage",
                url: "dang-ky",
                defaults: new { controller = "Manage", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Movie_Theater.Controllers" }
            );

            routes.MapRoute(
                name: "UserSignUp",
                url: "dang-ky",
                defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional },
                namespaces: new[] { "Movie_Theater.Controllers" }
            );

            routes.MapRoute(
                name: "UserSignIn",
                url: "dang-nhap",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "Movie_Theater.Controllers" }
            );

            routes.MapRoute(
                name: "ListMovie",
                url: "danh-sach-phim",
                defaults: new { controller = "ListMovie", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Movie_Theater.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Movie_Theater.Controllers" }
            );
        }
    }
}
