using Autofac;

namespace Qtec.AccountManagement.Web
{
    public class WebModule(string connectionString, string migrationAssembly) : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

        }
    }
}
