using RestaurantAPI.Entities;

namespace RestaurantAPI.Services
{
    public class MenuItemService : IMenuItemService
    {
        private static List<MenuItem> _menuItem = new List<MenuItem>();

        public List<MenuItem> GetAllMenuItems()
        {

            if (_menuItem.Count == 0)
            {
                _menuItem.AddRange
                    (
                      new MenuItem[]
                      {
                         new MenuItem(){ Id = 1,Name = "Cake - Cheese Cake 9 Inch",Price = 30},
                         new MenuItem(){ Id = 2,Name="Cheese - Swiss Sliced",Price = 31},
                         new MenuItem(){ Id = 3,Name="Trueblue - Blueberry",Price = 15},
                         new MenuItem(){ Id = 4,Name="Raisin - Dark",Price = 10},
                         new MenuItem(){ Id = 5,Name="Mix - Cappucino Cocktail",Price = 30},
                      }
                    );
            }

            return _menuItem;
        }
    }
}
