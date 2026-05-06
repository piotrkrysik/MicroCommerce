using EventBus.Messages.Events;
using MassTransit;
using Ordering.API.Entities;
using Ordering.API.Infrastructure.Persistence;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly OrderContext _dbContext;
        private readonly ILogger<BasketCheckoutConsumer> _logger;

        public BasketCheckoutConsumer(ILogger<BasketCheckoutConsumer> logger, OrderContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var message = context.Message;

            var orderEntity = new Order
            {
                UserName = message.UserName,
                TotalPrice = message.TotalPrice,
                FirstName = message.FirstName,
                LastName = message.LastName,
                EmailAddress = message.EmailAddress,
                AddressLine = message.AddressLine,
                Country = message.Country,
                CardName = message.CardName,
                CardNumber = message.CardNumber,
                Expiration = message.Expiration,
                CVV = message.Cvv,
                PaymentMethod = message.PaymentMethod
            };

            // Zapis do bazy danych
            _dbContext.Orders.Add(orderEntity);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Zamówienie zapisane pomyślnie w bazie SQL Server dla użytkownika: {userName}", message.UserName);
        }
    }
}