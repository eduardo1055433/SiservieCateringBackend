using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SiservieCatering.API.Models.Catalogo;
using SiservieCatering.API.Data;
using Microsoft.EntityFrameworkCore;
using Dapper;

namespace SiservieCatering.API.Controllers.Catalogo;

//[Authorize]
[ApiController]
[Route("api/catalogo")]
public class CatalogoController : ControllerBase
{
    private readonly AppDbContext _db;

    public CatalogoController(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Obtiene el catálogo de Puntos de Venta (POS) del esquema indicado.
    /// </summary>
    /// <param name="schema">Esquema PostgreSQL (Default: vidagong)</param>
    /// <returns>Lista de POS (Value=pos_hdserie, Label=pos_descri).</returns>
    [HttpGet("pos")]
    public async Task<ActionResult<IEnumerable<CatalogoItem>>> GetPos([FromQuery] string schema = "vidagong")
    {
        try
        {
            // Validación simple de esquema
           if (string.IsNullOrWhiteSpace(schema) || !System.Text.RegularExpressions.Regex.IsMatch(schema, "^[a-zA-Z0-9_]+$"))
                return BadRequest("Esquema inválido");

            var connection = _db.Database.GetDbConnection();
            // No abrimos explícitamente si Dapper maneja su propia conexión, pero Dapper usa IDbConnection.
            // EF Core connection puede estar cerrada.
            
            var sql = $"SELECT pos_hdserie as Value, pos_descri as Label FROM {schema}.pos";
            
            var result = await connection.QueryAsync<CatalogoItem>(sql);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { status = "Error", message = ex.Message });
        }
    }
    /// <summary>
    /// Obtiene el catálogo de Tipos de Cliente.
    /// </summary>
    /// <returns>Lista de tipos de cliente (Label/Value).</returns>
    [HttpGet("tipo-cliente")]
    public ActionResult<IEnumerable<CatalogoItem>> GetTipoCliente()
    {
        var lista = new List<CatalogoItem>
        {
            new CatalogoItem("TODOS", "TODOS"),
            new CatalogoItem("TRABAJADORES REGISTRADOS", "TRAB"),
            new CatalogoItem("VISITA", "Visita") 
        };

        return Ok(lista);
    }

    /// <summary>
    /// Obtiene el catálogo de Tipos de Venta.
    /// </summary>
    /// <returns>Lista de tipos de venta (Label/Value).</returns>
    [HttpGet("tipo-venta")]
    public ActionResult<IEnumerable<CatalogoItem>> GetTipoVenta()
    {
        var lista = new List<CatalogoItem>
        {
            new CatalogoItem("TODOS", "TODOS"),
            new CatalogoItem("BODEGA", "B"),
            new CatalogoItem("RAPIDO", "R")
        };

        return Ok(lista);
    }
}
