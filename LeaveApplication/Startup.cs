using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LeaveApplication.Startup))]
namespace LeaveApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
