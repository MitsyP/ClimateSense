using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelSecure.Core.Entities
{
    public class WeatherEntity
    {
       
            public string Name { get; set; } = null!;

            public string Country { get; set; } = null!;

            public decimal Temp { get; set; }

            public decimal FeelsLike { get; set; }

            public int Humidity { get; set; }

            public string Description { get; set; } = null!;

            public decimal WindSpeed { get; set; }

            public int Pressure { get; set; }

            public int Visibility { get; set; }

            public int Timezone { get; set; }

            public double Latitude { get; set; }

            public double Longitude { get; set; }
        
    }
}
