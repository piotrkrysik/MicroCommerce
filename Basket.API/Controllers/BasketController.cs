using Basket.API.Entities;
using Basket.API.Grpc;
using Basket.API.Repositories;
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

        public BasketController(IBasketRepository repository, ICatalogGrpcService catalogGrpcService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _catalogGrpcService = catalogGrpcService ?? throw new ArgumentNullException(nameof(catalogGrpcService));
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
