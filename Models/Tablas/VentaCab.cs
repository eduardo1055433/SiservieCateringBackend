using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiservieCatering.API.Models.Tablas;

[Table("ventas_cab")]
public class VentaCab
{
    [Key]
    [Column("vc_ticket")]
    [MaxLength(12)]
    public string VcTicket { get; set; } = string.Empty;

    [Column("user_id")]
    [MaxLength(10)]
    public string? UserId { get; set; }

    [Column("vc_emision_feho")]
    public DateTime? VcEmisionFeho { get; set; }

    [Column("vc_feed")]
    public int? VcFeed { get; set; }

    [Column("vc_anula_feho")]
    public DateTime? VcAnulaFeho { get; set; }

    [Column("vc_observacion")]
    [MaxLength(150)]
    public string? VcObservacion { get; set; }

    [Column("vc_anula_user_id")]
    [MaxLength(10)]
    public string? VcAnulaUserId { get; set; }

    [Column("vc_anulado")]
    public int? VcAnulado { get; set; }

    [Column("clie_docnum")]
    [MaxLength(15)]
    public string? ClieDocnum { get; set; }

    [Column("vc_total")]
    public decimal? VcTotal { get; set; }

    [Column("vc_subvencion")]
    public decimal? VcSubvencion { get; set; }

    [Column("vc_efectivo")]
    public decimal? VcEfectivo { get; set; }

    [Column("vc_acuenta")]
    public decimal? VcAcuenta { get; set; }

    [Column("vc_vuelto")]
    public decimal? VcVuelto { get; set; }

    [Column("vc_pagar")]
    public decimal? VcPagar { get; set; }

    [Column("vc_credito")]
    public decimal? VcCredito { get; set; }

    [Column("serv_id")]
    [MaxLength(15)]
    public string ServId { get; set; } = string.Empty;

    [Column("sede_id")]
    [MaxLength(10)]
    public string? SedeId { get; set; }

    [Column("pos_hdserie")]
    [MaxLength(20)]
    public string? PosHdserie { get; set; }

    [Column("cjt_id")]
    public int? CjtId { get; set; }

    [Column("vc_modo")]
    [MaxLength(2)]
    public string? VcModo { get; set; }

    [Column("inv_id")]
    public int? InvId { get; set; }
}
