using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;

namespace ReadFacade
{
    public static class TypeRegistrations
    {
        public static void RegisterTypes(this IServiceCollection services)
        {
            services.RegisterAssemblyPublicNonGenericClasses()
                .AsPublicImplementedInterfaces();
        }
    }
}
