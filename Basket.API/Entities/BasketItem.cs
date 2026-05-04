namespace Basket.API.Entities
{
    public class BasketItem
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
    }
}
