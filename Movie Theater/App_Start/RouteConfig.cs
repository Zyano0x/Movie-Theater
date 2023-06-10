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
              name: "Edit/Add Seats",
              url: "chinh-sua",
              defaults: new { controller = "Orders", action = "Edit", id = UrlParameter.Optional },
              namespaces: new[] { "Movie_Theater.Controllers" }
          );

            routes.MapRoute(
               name: "Checkout",
               url: "thanh-toan",
               defaults: new { controller = "Orders", action = "Checkout", id = UrlParameter.Optional },
               namespaces: new[] { "Movie_Theater.Controllers" }
           );

            routes.MapRoute(
               name: "Order",
               url: "thong-tin-ve",
               defaults: new { controller = "Orders", action = "Details", id = UrlParameter.Optional },
               namespaces: new[] { "Movie_Theater.Controllers" }
           );

            routes.MapRoute(
               name: "Ticket",
               url: "dat-ve/{name}",
               defaults: new { controller = "Tickets", action = "Create", name = UrlParameter.Optional },
               namespaces: new[] { "Movie_Theater.Controllers" }
           );

            routes.MapRoute(
               name: "BookingHistory",
               url: "lich-su-dat-ve",
               defaults: new { controller = "Booking", action = "History", id = UrlParameter.Optional },
               namespaces: new[] { "Movie_Theater.Controllers" }
           );

            routes.MapRoute(
               name: "Booking",
               url: "b/{name}",
               defaults: new { controller = "Booking", action = "Create", name = UrlParameter.Optional },
               namespaces: new[] { "Movie_Theater.Controllers" }
           );

            routes.MapRoute(
               name: "Review",
               url: "r/{id}",
               defaults: new { controller = "Movies", action = "Review", id = UrlParameter.Optional },
               namespaces: new[] { "Movie_Theater.Controllers" }
           );

            routes.MapRoute(
                name: "NewsArticle",
                url: "article/{name}",
                defaults: new { controller = "News", action = "Details", name = UrlParameter.Optional },
                namespaces: new[] { "Movie_Theater.Controllers" }
            );

            routes.MapRoute(
                name: "NewsList",
                url: "tin-tuc",
                defaults: new { controller = "News", action = "Index", id = UrlParameter.Optional },
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
               name: "ChangePassword",
               url: "doi-mat-khau",
               defaults: new { controller = "Manages", action = "ChangePassword", id = UrlParameter.Optional },
               namespaces: new[] { "Movie_Theater.Controllers" }
           );

            routes.MapRoute(
                name: "UserManage",
                url: "quan-ly-tai-khoan",
                defaults: new { controller = "Manages", action = "Index", id = UrlParameter.Optional },
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
                name: "ShowMovies",
                url: "danh-sach-phim",
                defaults: new { controller = "Movies", action = "ShowMovies", id = UrlParameter.Optional },
                namespaces: new[] { "Movie_Theater.Controllers" }
            );

            routes.MapRoute(
               name: "Homepage",
               url: "homepage",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
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
