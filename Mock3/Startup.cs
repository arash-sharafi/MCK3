using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mock3.Startup))]
namespace Mock3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
