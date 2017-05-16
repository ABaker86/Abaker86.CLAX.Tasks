using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Abaker86.CLAX.Tasks.Startup))]
namespace Abaker86.CLAX.Tasks
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
