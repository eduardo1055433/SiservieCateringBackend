using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiservieCatering.API.Data;
using SiservieCatering.API.Models.Tablas;
using System.Text.RegularExpressions;

namespace SiservieCatering.API.Controllers.Tablas;

[ApiController]
[Route("api/tablas/[controller]")]
public class VentasCabController : ControllerBase
{
    private readonly AppDbContext _db;

    public VentasCabController(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Obtiene todas las ventas (cabecera) del esquema indicado.
    /// </summary>
    /// <param name="schema">Nombre del esquema PostgreSQL (ej: vidagong)</param>
    [HttpGet("{schema}")]
    public async Task<ActionResult<IEnumerable<VentaCab>>> GetBySchema(string schema)
    {
        try
        {
            if (!Regex.IsMatch(schema, @"^[a-zA-Z_][a-zA-Z0-9_]*$"))
                return BadRequest(new { status = "Error", message = "Nombre de esquema inválido." });

            var ventas = await _db.VentasCab
                .FromSqlRaw($"SELECT * FROM \"{schema}\".ventas_cab")
                .ToListAsync();

            return Ok(ventas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { status = "Error", message = ex.Message });
        }
    }
}
