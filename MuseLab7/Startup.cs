using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MuseLab7.Startup))]
namespace MuseLab7
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
