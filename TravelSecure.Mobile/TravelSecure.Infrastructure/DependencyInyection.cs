using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TravelSecure.Infrastructure
{
    public static class DependencyInyection
    {
        public static IServiceCollection AddInfrestructureDI(this IServiceCollection services)
        {
            return services;
        }
    }
}