using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChannakyaAccounting.Startup))]
namespace ChannakyaAccounting
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
