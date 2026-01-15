using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TierListes.Infrastructure.Persistence;

namespace TierListes.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public HealthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(
            new
            {
                status = "healthy",
                message = "Backend TierListes .NET 9 fonctionne !",
                timestamp = DateTime.UtcNow,
                version = "1.0.0"
            }
        );
    }

    [HttpGet("database")]
    public async Task<IActionResult> CheckDatabase()
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync();

            if (canConnect)
            {
                return Ok(
                    new
                    {
                        status = "connected",
                        message = "Connexion à MySQL réussie !",
                        database = "tierlistesdb",
                        server = "localhost:3307"
                    }
                );
            }
            else
            {
                return StatusCode(
                    503,
                    new
                    {
                        status = "error",
                        message = "Impossible de se connecter à la base de données"
                    }
                );
            }
        }
        catch (Exception ex)
        {
            return StatusCode(
                503,
                new
                {
                    status = "error",
                    message = "Erreur lors de la connexion à la base de données",
                    error = ex.Message
                }
            );
        }
    }
}
