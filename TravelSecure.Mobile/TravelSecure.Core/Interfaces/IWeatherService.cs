using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelSecure.Core.Interfaces
{
    public interface IWeatherService
    {
        Task<string> ObtenerClima(string ciudad);
    }
}
