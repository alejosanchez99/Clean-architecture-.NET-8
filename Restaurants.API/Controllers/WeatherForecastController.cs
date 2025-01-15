using Microsoft.AspNetCore.Mvc;

namespace Restaurants.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController(IWeatherForecastService weatherForecastService) : ControllerBase
    {
        private readonly IWeatherForecastService _weatherForecastService = weatherForecastService;

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return _weatherForecastService.Get();
        }
    }
}
