using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EditThor1.Startup))]
namespace EditThor1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
