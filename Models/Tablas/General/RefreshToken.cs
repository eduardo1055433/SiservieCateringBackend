using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiservieCatering.API.Models.Tablas.General;

[Table("refresh_tokens", Schema = "general")]
public class RefreshToken
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    [MaxLength(50)]
    public string UserId { get; set; } = string.Empty;

    [Column("token")]
    [MaxLength(255)]
    public string Token { get; set; } = string.Empty;

    [Column("expires")]
    public DateTime Expires { get; set; }

    [Column("created")]
    public DateTime Created { get; set; }

    [Column("revoked")]
    public DateTime? Revoked { get; set; }

    [ForeignKey("UserId")]
    public Usuario? Usuario { get; set; }

    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsActive => Revoked == null && !IsExpired;
}
