using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ReservaDeAulas.Startup))]
namespace ReservaDeAulas
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
