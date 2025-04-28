using NetCore.AutoRegisterDi;

namespace API
{
    public static class TypeRegistrations
    {
        public static void RegisterTypes(this IServiceCollection services)
        {
            services.RegisterAssemblyPublicNonGenericClasses()
                .AsPublicImplementedInterfaces();

            Integrations.TypeRegistrations.RegisterTypes(services);
            ReadFacade.TypeRegistrations.RegisterTypes(services);
            WriteFacade.TypeRegistrations.RegisterTypes(services);
        }
    }
}
