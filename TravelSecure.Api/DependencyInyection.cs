using TravelSecure.Aplication;
using TravelSecure.Infrastructure;

namespace TravelSecure.Api
{
    public static class DependencyInyection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection services)
        {
            services.AddAplicationDI().AddInfrestructureDI();
            return services;
        }
    }
}
