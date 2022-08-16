using Microsoft.AspNetCore.Mvc;
using WSVenta.Models;

namespace WSVenta.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            List<WeatherForecast> lForecasts = new List<WeatherForecast>();
            lForecasts.Add(new WeatherForecast() { id = 5, Name = "Aless" });
            lForecasts.Add(new WeatherForecast() { id = 1, Name = "Chris" });

            return lForecasts;
        }
    }
}