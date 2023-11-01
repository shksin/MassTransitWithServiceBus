namespace OrderContract
{
    public class Order
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
