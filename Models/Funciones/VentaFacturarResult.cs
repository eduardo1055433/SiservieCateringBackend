namespace SiservieCatering.API.Models.Funciones;

/// <summary>
/// Resultado de la función base.ventas_facturar_x_ticket
/// </summary>
public class VentaFacturarResult
{
    public DateTime? EmisionFeho { get; set; }
    public string? Ticket { get; set; }
    public string? Origen { get; set; }
    public decimal? Total { get; set; }
    public decimal? Subvencion { get; set; }
    public decimal? APagar { get; set; }
    public decimal? ACuenta { get; set; }
    public decimal? Credito { get; set; }
    public decimal? AboLibre { get; set; }
    public string? ClieDocnum { get; set; }
    public string? ClieCodsap { get; set; }
    public string? CliePat { get; set; }
    public string? ClieMat { get; set; }
    public string? ClieNom { get; set; }
    public string? Anyomes { get; set; }
    public string? UnidDescri { get; set; }
    public string? ZonaDescri { get; set; }
    public string? SubdivDescri { get; set; }
    public string? CatDescri { get; set; }
    public string? CcostoDescri { get; set; }
    public string? VcModo { get; set; }
}
