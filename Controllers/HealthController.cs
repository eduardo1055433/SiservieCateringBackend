using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiservieCatering.API.Data;

namespace SiservieCatering.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _db;

    public HealthController(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Verifica la conexión a la base de datos PostgreSQL.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var connection = _db.Database.GetDbConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT NOW()";
            var result = await command.ExecuteScalarAsync();

            return Ok(new
            {
                status = "OK",
                database = "Connected",
                serverTime = result?.ToString()
            });
        }
        catch (Exception ex)
        {
            return StatusCode(503, new
            {
                status = "Error",
                database = "Disconnected",
                message = ex.Message
            });
        }
    }
}
