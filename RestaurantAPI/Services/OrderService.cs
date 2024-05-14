using RestaurantAPI.Entities;

namespace RestaurantAPI.Services
{
    public class OrderService : IOrderService
    {
        private static List<Order> _orders = new List<Order>();

        public List<Order> GetOrderByCustomerId(int customerId)
        {
            return _orders.Where(x => x.CustomerID == customerId).ToList();
        }

        public void PlaceOrder(Order order)
        {
            _orders.Add(order);
        }

        public void PlaceOrder(List<Order> orders)
        {
            orders.ForEach(x => _orders.Add(x));
        }
    }
}
