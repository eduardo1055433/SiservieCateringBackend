using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiservieCatering.API.Models.Tablas.General;

[Table("empresas", Schema = "general")]
public class Empresa
{
    [Key]
    [Column("emp_schema")]
    [MaxLength(20)]
    public string EmpSchema { get; set; } = string.Empty;

    [Column("emp_nombre")]
    [MaxLength(30)]
    public string EmpNombre { get; set; } = string.Empty;

    [Column("emp_predet")]
    public decimal? EmpPredet { get; set; }
}
