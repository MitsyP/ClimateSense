using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelSecure.Core
{
    public static class DependencyInyection
    {
        public static IServiceCollection AddCoreDI(this IServiceCollection services)
        {
            return services;
        }
    }
}
