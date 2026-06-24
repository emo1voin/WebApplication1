using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly OrdersDbContext _context;

    public ClientsController(OrdersDbContext context)
    {
        _context = context;
    }

    // GET api/clients - Get all clients for order creation
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetClients()
    {
        return await _context.Users.ToListAsync();
    }
}
