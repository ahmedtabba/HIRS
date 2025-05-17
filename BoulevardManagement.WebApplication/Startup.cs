using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BoulevardManagement.WebApplication.Startup))]
namespace BoulevardManagement.WebApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
