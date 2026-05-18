using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelSecure.Aplication
{
    public static class DependencyInyection
    {
        public static IServiceCollection AddAplicationDI(this IServiceCollection services)
        {
             // Registrar MediatR
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(
                    typeof(DependencyInyection).Assembly));

            // Registrar HttpClient
            services.AddHttpClient();

            return services;
        }
    }
}
