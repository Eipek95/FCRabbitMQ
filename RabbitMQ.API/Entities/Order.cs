namespace RabbitMQ.API.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public DateTime OrderTime { get; set; }
        public List<OrderItem> Items { get; set; }
    }

}