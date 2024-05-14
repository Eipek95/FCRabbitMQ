using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Entities;
using RestaurantAPI.Services;
using RestaurantAPI.Services.RabbitMQServices;
using Shared;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly RabbitMQPublisher _rabbitMQPublisher;
        public OrdersController(OrderService orderService, RabbitMQPublisher rabbitMQPublisher)
        {
            _orderService = orderService;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        [HttpGet("{id}")]

        public IActionResult Get(int id)
        {
            var result = _orderService.GetOrderByCustomerId(id);
            if (!result.Any()) return NotFound("Not Found");
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Add(Order order)
        {
            _orderService.PlaceOrder(order);

            List<CreateOrderItem> createOrderItems = order.Items.Select(item => new CreateOrderItem
            {
                MenuItemId = item.MenuItemId,
                Quantity = item.Quantity
            }).ToList();


            _rabbitMQPublisher.Publish(new CreateOrderMessage()
            {
                CustomerID = order.CustomerID,
                Items = createOrderItems
            });
            return Ok("Your Order Has Been Saved And Successfully Added To The Queue");
        }

        [HttpPost("AddRange")]
        public IActionResult Add(List<Order> order)
        {
            _orderService.PlaceOrder(order);
            return Ok();
        }
    }
}


//[
//  {
//    "customerID": 1,
//    "items": [
//      {
//        "menuItemId": 1,
//        "quantity": 2
//      }
//    ]
//  },
//{
//    "customerID": 1,
//    "items": [
//      {
//        "menuItemId": 2,
//        "quantity": 1
//      }
//    ]
//  }
//]