using RestaurantAPI.Entities;

namespace RestaurantAPI.Services
{
    public interface IOrderService
    {
        void PlaceOrder(Order order);
        void PlaceOrder(List<Order> order);
        List<Order> GetOrderByCustomerId(int customerId);
    }
}
