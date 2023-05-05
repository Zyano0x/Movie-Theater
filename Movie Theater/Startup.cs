using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Movie_Theater.Startup))]
namespace Movie_Theater
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
