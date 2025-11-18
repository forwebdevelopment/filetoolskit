using Filetoolkits.application.Product;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Filetoolkits.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private ISender _sender;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ISender sender)
        {
            _logger = logger;
            _sender = sender;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {

            _sender.Send(new ProductQuery("Krishna Mohan"));


            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
