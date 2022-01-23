using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Forca.Startup))]
namespace Forca
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
