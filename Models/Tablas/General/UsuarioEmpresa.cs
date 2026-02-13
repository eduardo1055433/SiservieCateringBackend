using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiservieCatering.API.Models.Tablas.General;

[Table("usuario_empresa", Schema = "general")]
public class UsuarioEmpresa
{
    [Column("user_id")]
    [MaxLength(50)]
    public string UserId { get; set; } = string.Empty;

    [Column("emp_schema")]
    [MaxLength(20)]
    public string EmpSchema { get; set; } = string.Empty;

    [Column("rol_id")]
    public int RolId { get; set; }

    [Column("ue_activo")]
    public short? UeActivo { get; set; }

    [Column("ue_predeterminado")]
    public short? UePredeterminado { get; set; }

    [ForeignKey("UserId")]
    public Usuario? Usuario { get; set; }

    [ForeignKey("EmpSchema")]
    public Empresa? Empresa { get; set; }

    [ForeignKey("RolId")]
    public Rol? Rol { get; set; }
}
