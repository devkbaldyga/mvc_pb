using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebApplication44.Startup))]
namespace WebApplication44
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
