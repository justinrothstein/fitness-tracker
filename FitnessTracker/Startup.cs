using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FitnessTracker.Startup))]
namespace FitnessTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
