using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebSignalR.Startup))]
namespace WebSignalR
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
