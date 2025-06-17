using Autofac;
using Qtec.AccountManagement.Application.Services;
using Qtec.AccountManagement.Domain;
using Qtec.AccountManagement.Domain.RepositoryContracts;
using Qtec.AccountManagement.Infrastructure.Repositories;
using Qtec.AccountManagement.Infrastructure.UnitOfWorks;
using System.Data.SqlClient;

namespace Qtec.AccountManagement.Web
{
    public class WebModule(string connectionString) : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SqlConnection>()
           .AsSelf()
           .WithParameter("connectionString", connectionString)
           .InstancePerLifetimeScope();

            builder.RegisterType<RoleRepository>()
                .As<IRoleRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AccountRepository>()
                .As<IAccountRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .WithParameter("connectionString", connectionString)
                .InstancePerLifetimeScope();

            builder.RegisterType<AccountManagementService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserManagementService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RoleManagementService>()
                .InstancePerLifetimeScope();
        }
    }
}
