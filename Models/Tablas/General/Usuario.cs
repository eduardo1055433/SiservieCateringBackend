using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiservieCatering.API.Models.Tablas.General;

[Table("usuarios", Schema = "general")]
public class Usuario
{
    [Key]
    [Column("user_id")]
    [MaxLength(50)]
    public string UserId { get; set; } = string.Empty;

    [Column("user_email")]
    [MaxLength(100)]
    public string UserEmail { get; set; } = string.Empty;

    [Column("user_password")]
    [MaxLength(255)]
    public string UserPassword { get; set; } = string.Empty;

    [Column("user_nombre")]
    [MaxLength(100)]
    public string? UserNombre { get; set; }

    [Column("user_activo")]
    public short? UserActivo { get; set; }
}
