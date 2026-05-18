using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TravelSecure.Core.Entities;

namespace TravelSecure.Aplication.Queries
{
    // QUERY
    public record GetWeatherQuery(string Ciudad)
        : IRequest<WeatherEntity>;

    // HANDLER
    public class GetWeatherQueryHandler
        : IRequestHandler<GetWeatherQuery, WeatherEntity>
    {
        private readonly HttpClient _httpClient;

        private const string ApiKey = "e6ea39e041d47919021a46041574afe1";

        public GetWeatherQueryHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<WeatherEntity> Handle(
            GetWeatherQuery request,
            CancellationToken cancellationToken)
        {
            string url =
                $"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(request.Ciudad)}&appid={ApiKey}&units=metric&lang=es";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error al consultar OpenWeather");
            }

            string json = await response.Content.ReadAsStringAsync();

            using JsonDocument doc = JsonDocument.Parse(json);

            var root = doc.RootElement;

            var entity = new WeatherEntity
            {
                Name = root.GetProperty("name").GetString()!,

                Country = root
                    .GetProperty("sys")
                    .GetProperty("country")
                    .GetString()!,

                Temp = root
                    .GetProperty("main")
                    .GetProperty("temp")
                    .GetDecimal(),

                FeelsLike = root
                    .GetProperty("main")
                    .GetProperty("feels_like")
                    .GetDecimal(),

                Humidity = root
                    .GetProperty("main")
                    .GetProperty("humidity")
                    .GetInt32(),

                Description = root
                    .GetProperty("weather")[0]
                    .GetProperty("description")
                    .GetString()!,

                WindSpeed = root
                    .GetProperty("wind")
                    .GetProperty("speed")
                    .GetDecimal(),

                Pressure = root
                    .GetProperty("main")
                    .GetProperty("pressure")
                    .GetInt32(),

                Visibility = root
                    .GetProperty("visibility")
                    .GetInt32(),

                Timezone = root
                    .GetProperty("timezone")
                    .GetInt32(),

                Latitude = root
                    .GetProperty("coord")
                    .GetProperty("lat")
                    .GetDouble(),

                Longitude = root
                    .GetProperty("coord")
                    .GetProperty("lon")
                    .GetDouble()
            };

            return entity;
        }
    }
}


/*
 
 public async Task<string> Handle(
            GetWeatherQuery request,
            CancellationToken cancellationToken)
        {
            // lógica aquí
        }
 
 */