using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Models
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // Redirect to the custom login URL for the admin area
                filterContext.Result = new RedirectResult("/Admin/Account/Login");
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}