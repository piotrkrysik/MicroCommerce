using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class BasketCheckoutEvent
    {
        // Dane użytkownika
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }

        // Dane do wysyłki
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine { get; set; }
        public string Country { get; set; }

        // Dane płatności (uproszczone)
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string Cvv { get; set; } = string.Empty;
        public int PaymentMethod { get; set; }
    }
}
