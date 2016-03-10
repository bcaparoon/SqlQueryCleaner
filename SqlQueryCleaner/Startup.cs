using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SqlQueryCleaner.Startup))]
namespace SqlQueryCleaner
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
