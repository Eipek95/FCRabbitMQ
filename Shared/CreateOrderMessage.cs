namespace Shared
{
    public class CreateOrderMessage
    {
        public int CustomerID { get; set; }
        public List<CreateOrderItem> Items { get; set; }
    }
    public class CreateOrderItem
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
    }
}
