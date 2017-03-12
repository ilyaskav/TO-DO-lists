using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RubyTest.Startup))]
namespace RubyTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
