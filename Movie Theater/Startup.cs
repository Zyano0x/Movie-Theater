using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Movie_Theater.Models;
using Owin;

[assembly: OwinStartupAttribute(typeof(Movie_Theater.Startup))]
namespace Movie_Theater
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!RoleManager.RoleExists("Adminstrator"))
            {

                // first we create Admin rool    
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Adminstrator";
                RoleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                   

                var user = new ApplicationUser();
                user.UserName = "Zyano";
                user.Email = "nthdm00@gmail.com";
                string userPWD = "0x1337";

                var checkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin    
                if (checkUser.Succeeded)
                {
                    var result = UserManager.AddToRole(user.Id, "Adminstrator");
                }
            }

            // creating Creating Manager role     
            if (!RoleManager.RoleExists("Staff"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Staff";
                RoleManager.Create(role);
            }

            if (!RoleManager.RoleExists("User"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "User";
                RoleManager.Create(role);
            }
        }
    }
}
