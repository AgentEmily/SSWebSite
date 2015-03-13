using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SStest.Startup))]
namespace SStest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
