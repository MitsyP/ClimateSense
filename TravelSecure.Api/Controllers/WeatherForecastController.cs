using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace TravelSecure.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForescastController : ControllerBase
    {
        private readonly HttpClient _httpClient;


        private const string ApiKey = "e6ea39e041d47919021a46041574afe1";

        
        public WeatherForescastController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        
        [HttpGet("ruta/{ciudad}")]
        public async Task<IActionResult> GetClimaPorRuta(string ciudad)
        {
            
            if (string.IsNullOrWhiteSpace(ciudad))
            {
                return BadRequest("El nombre de la ciudad es requerido.");
            }

            try
            {
                string url = $"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(ciudad)}&appid={ApiKey}&units=metric&lang=es";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    
                    return StatusCode((int)response.StatusCode, $"Error al consultar OpenWeather: {response.ReasonPhrase}");
                }

                string jsonPlano = await response.Content.ReadAsStringAsync();

                
                return Content(jsonPlano, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno en el servidor: {ex.Message}");
            }
        }
    }
}