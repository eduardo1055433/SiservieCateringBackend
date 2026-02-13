using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiservieCatering.API.Data;
using SiservieCatering.API.Models.Funciones;
using Microsoft.AspNetCore.Authorization;

namespace SiservieCatering.API.Controllers.Funciones;

[Authorize]
[ApiController]
[Route("api/funciones/[controller]")]
public class VentasFacturarController : ControllerBase
{
    private readonly AppDbContext _db;

    public VentasFacturarController(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Ejecuta la función base.ventas_facturar_x_ticket con los parámetros indicados.
    /// </summary>
    /// <param name="schema">Esquema PostgreSQL (ej: vidagong)</param>
    /// <param name="ticket">Filtro de ticket (TODOS = sin filtro)</param>
    /// <param name="fechaInicio">Fecha inicio (ej: 2024-08-01)</param>
    /// <param name="fechaFin">Fecha fin (ej: 2025-08-31)</param>
    /// <param name="filtro5">Filtro 5 (TODOS = sin filtro)</param>
    /// <param name="filtro6">Filtro 6 (TODOS = sin filtro)</param>
    /// <param name="filtro7">Filtro 7 (TODOS = sin filtro)</param>
    /// <param name="filtro8">Filtro 8 (TODOS = sin filtro)</param>
    /// <param name="filtro9">Filtro 9 (TODOS = sin filtro)</param>
    /// <param name="filtro10">Filtro 10 (TODOS = sin filtro)</param>
    /// <param name="filtro11">Filtro 11 (TODOS = sin filtro)</param>
    /// <param name="filtro12">Filtro 12 (TODOS = sin filtro)</param>
    /// <param name="filtro13">Filtro 13 (TODOS = sin filtro)</param>
    /// <param name="tipoProd">Tipo de producto (ej: NOPROD)</param>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VentaFacturarResult>>> Get(
        [FromQuery] string schema = "vidagong",
        [FromQuery] string ticket = "TODOS",
        [FromQuery] string fechaInicio = "2024-08-01",
        [FromQuery] string fechaFin = "2025-08-31",
        [FromQuery] string filtro5 = "TODOS",
        [FromQuery] string filtro6 = "TODOS",
        [FromQuery] string filtro7 = "TODOS",
        [FromQuery] string filtro8 = "TODOS",
        [FromQuery] string filtro9 = "TODOS",
        [FromQuery] string filtro10 = "TODOS",
        [FromQuery] string filtro11 = "TODOS",
        [FromQuery] string filtro12 = "TODOS",
        [FromQuery] string filtro13 = "TODOS",
        [FromQuery] string tipoProd = "NOPROD")
    {
        try
        {
            var connection = _db.Database.GetDbConnection();
            await connection.OpenAsync();

            var sql = @"
                SELECT * FROM base.ventas_facturar_x_ticket(
                    @Schema, @Ticket, @FechaInicio::date, @FechaFin::date,
                    @Filtro5, @Filtro6, @Filtro7, @Filtro8, @Filtro9,
                    @Filtro10, @Filtro11, @Filtro12, @Filtro13, @TipoProd
                ) AS (
                    emision_feho timestamp without time zone,
                    ticket character varying,
                    origen varchar(10),
                    total numeric(12,2),
                    subvencion numeric(12,2),
                    a_pagar numeric(12,2),
                    a_cuenta numeric(12,2),
                    credito numeric(12,2),
                    abo_libre numeric(12,2),
                    clie_docnum varchar(15),
                    clie_codsap varchar(15),
                    clie_pat varchar(25),
                    clie_mat varchar(25),
                    clie_nom varchar(30),
                    anyomes varchar,
                    unid_descri varchar(50),
                    zona_descri varchar(50),
                    subdiv_descri varchar(50),
                    cat_descri varchar(50),
                    ccosto_descri varchar(60),
                    vc_modo varchar(1)
                )";

            var parameters = new
            {
                Schema = schema,
                Ticket = ticket,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Filtro5 = filtro5,
                Filtro6 = filtro6,
                Filtro7 = filtro7,
                Filtro8 = filtro8,
                Filtro9 = filtro9,
                Filtro10 = filtro10,
                Filtro11 = filtro11,
                Filtro12 = filtro12,
                Filtro13 = filtro13,
                TipoProd = tipoProd
            };

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            var result = await connection.QueryAsync<VentaFacturarResult>(sql, parameters);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { status = "Error", message = ex.Message });
        }
    }
}
