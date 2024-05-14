using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly MenuItemService _menuItemService;

        public MenuItemsController(MenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        [HttpGet]

        public IActionResult Get()
        {
            var result = _menuItemService.GetAllMenuItems();
            return Ok(result);
        }
    }
}
