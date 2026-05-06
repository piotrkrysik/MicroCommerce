using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ordering.API.Infrastructure.Persistence;
using System.Net;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderContext _context;

        public OrderController(OrderContext context)
        {
            _context = context;
        }

        [HttpGet("{userName}", Name = "GetOrder")]
        public async Task<ActionResult> GetOrdersByUserName(string userName)
        {
            var orders = await _context.Orders
                .Where(o => o.UserName == userName)
                .ToListAsync();

            return Ok(orders);
        }
    }
}