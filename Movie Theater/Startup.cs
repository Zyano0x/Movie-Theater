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
                user.IsEnabled = true;
                user.EmailConfirmed = true;
                string userPWD = "0x1337";

                var user1 = new ApplicationUser();
                user1.UserName = "DriftMan";
                user1.Email = "duypham22102@gmail.com";
                user1.IsEnabled = true;
                user1.EmailConfirmed = true;
                string user1PWD = "Aa@123";

                var checkUser = UserManager.Create(user, userPWD);
                var checkUser1 = UserManager.Create(user1, user1PWD);

                //Add default User to Role Admin    
                if (checkUser.Succeeded && checkUser1.Succeeded)
                {
                    var result = UserManager.AddToRole(user.Id, "Adminstrator");
                    var result1 = UserManager.AddToRole(user1.Id, "Adminstrator");
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
