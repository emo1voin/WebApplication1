using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PickupPointsController : ControllerBase
{
    private readonly OrdersDbContext _context;

    public PickupPointsController(OrdersDbContext context)
    {
        _context = context;
    }

    // GET api/pickup-points - Get all pickup points
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PickupPoint>>> GetPickupPoints()
    {
        return await _context.PickupPoints.ToListAsync();
    }
}
