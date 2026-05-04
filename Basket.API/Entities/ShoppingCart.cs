namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        // To jest nasz klucz w Redisie (np. "piotr")
        public string UserName { get; set; } = string.Empty;

        // Lista przedmiotów w koszyku
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();

        public ShoppingCart()
        {
        }

        public ShoppingCart(string userName)
        {
            UserName = userName;
        }

        // Logika biznesowa: automatyczne wyliczanie sumy koszyka
        public decimal TotalPrice
        {
            get
            {
                return Items.Sum(item => item.Price * item.Quantity);
            }
        }
    }
}
