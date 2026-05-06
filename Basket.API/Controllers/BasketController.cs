using Basket.API.Entities;
using Basket.API.Grpc;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using MassTransit.Transports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly ICatalogGrpcService _catalogGrpcService;
        private readonly IPublishEndpoint _publishEndpoint;
        public BasketController(IBasketRepository repository, ICatalogGrpcService catalogGrpcService, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _catalogGrpcService = catalogGrpcService;
            _publishEndpoint = publishEndpoint;
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // 1. Pobierz istniejący koszyk z Redisa
            var basket = await _repository.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }

            // 2. Stwórz BasketCheckoutEvent (używamy AutoMappera lub ręcznie przepisujemy pola)
            var eventMessage = new BasketCheckoutEvent
            {
                UserName = basket.UserName,
                TotalPrice = basket.TotalPrice,
                FirstName = basketCheckout.FirstName,
                LastName = basketCheckout.LastName,
                EmailAddress = basketCheckout.EmailAddress,
                AddressLine = basketCheckout.AddressLine,
                Country = basketCheckout.Country,
                CardName = basketCheckout.CardName,
                CardNumber = basketCheckout.CardNumber,
                Expiration = basketCheckout.Expiration,
                Cvv = basketCheckout.CVV, // Tu użyj takiej nazwy, jaką masz w opcji B
                PaymentMethod = basketCheckout.PaymentMethod
            };

            // 3. Wyślij event do RabbitMQ (MassTransit zajmie się resztą)
            await _publishEndpoint.Publish(eventMessage);

            // 4. Usuń koszyk z Redisa (użytkownik właśnie złożył zamówienie)
            await _repository.DeleteBasket(basket.UserName);

            return Accepted();
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasket(userName);
            // Jeśli koszyk jest pusty, zwracamy nowy, pusty koszyk dla tego użytkownika
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            foreach (var item in basket.Items)
            {
                // Dzwonimy do Katalogu po aktualną cenę i nazwę
                var product = await _catalogGrpcService.GetProduct(item.ProductId);

                // Aktualizujemy dane w koszyku tymi z Katalogu
                item.Price = (decimal)product.Price;
                item.ProductName = product.Name;
            }

            return Ok(await _repository.UpdateBasket(basket));
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }
    }
}
