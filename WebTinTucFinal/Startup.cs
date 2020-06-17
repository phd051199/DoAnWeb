using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebTinTucFinal.Startup))]
namespace WebTinTucFinal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
