using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiservieCatering.API.Models.Tablas.General;

[Table("roles", Schema = "general")]
public class Rol
{
    [Key]
    [Column("rol_id")]
    public int RolId { get; set; }

    [Column("rol_descripcion")]
    [MaxLength(50)]
    public string RolDescripcion { get; set; } = string.Empty;
}
