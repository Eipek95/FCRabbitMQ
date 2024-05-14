using Microsoft.AspNetCore.Mvc;
using RestaurantWEB.Models;
using RestaurantWEB.Services;
using System.Diagnostics;

namespace RestaurantWEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RabbitMQService _rabbitmqService;

        public HomeController(ILogger<HomeController> logger, RabbitMQService rabbitmqService)
        {
            _logger = logger;
            _rabbitmqService = rabbitmqService;
        }

        public IActionResult Index()
        {
            _rabbitmqService.ConsumeQueue();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
