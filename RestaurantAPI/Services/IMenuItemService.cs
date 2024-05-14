using RestaurantAPI.Entities;

namespace RestaurantAPI.Services
{
    public interface IMenuItemService
    {
        List<MenuItem> GetAllMenuItems();
    }
}
