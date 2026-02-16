using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiservieCatering.API.Data;
using SiservieCatering.API.Models.Tablas.General;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SiservieCatering.API.Controllers.General;

public record LoginRequest(string UserId, string Password);
public record LoginResponse(string UserId, string Nombre, string Token, string RefreshToken, List<EmpresaAccess> Empresas);
public record EmpresaAccess(string Schema, string NombreEmpresa, string Rol, bool Predeterminado);
public record RefreshTokenRequest(string Token, string RefreshToken);

[ApiController]
[Route("api/general/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    /// <summary>
    /// Inicia sesión y devuelve Access Token (corto) + Refresh Token (largo).
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        // 1. Buscar usuario
        var usuario = await _db.Usuarios
            .FirstOrDefaultAsync(u => u.UserId == request.UserId && u.UserActivo == 1);

        if (usuario == null)
            return Unauthorized(new { status = "Error", message = "Usuario o contraseña incorrectos." });

        // 2. Validar contraseña con BCrypt
        if (!BCrypt.Net.BCrypt.Verify(request.Password, usuario.UserPassword))
             return Unauthorized(new { status = "Error", message = "Usuario o contraseña incorrectos." });

        // 3. Obtener empresas y roles
        var accesos = await _db.UsuarioEmpresas
            .Include(ue => ue.Empresa)
            .Include(ue => ue.Rol)
            .Where(ue => ue.UserId == usuario.UserId && ue.UeActivo == 1)
            .Select(ue => new EmpresaAccess(
                ue.EmpSchema, 
                ue.Empresa!.EmpNombre, 
                ue.Rol!.RolDescripcion, 
                ue.UePredeterminado == 1
            ))
            .ToListAsync();

        if (!accesos.Any())
             return Unauthorized(new { status = "Error", message = "El usuario no tiene empresas asignadas." });

        // 4. Generar Tokens
        var jwtToken = GenerateJwtToken(usuario);
        var refreshToken = GenerateRefreshToken();

        // 5. Guardar Refresh Token en BD
        var rtEntity = new RefreshToken
        {
            UserId = usuario.UserId,
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(7), // Dura 7 días
            Created = DateTime.UtcNow
        };
        _db.RefreshTokens.Add(rtEntity);
        await _db.SaveChangesAsync();

        return Ok(new LoginResponse(usuario.UserId, usuario.UserNombre ?? "", jwtToken, refreshToken, accesos));
    }

    /// <summary>
    /// Renueva el Access Token usando un Refresh Token válido.
    /// </summary>
    [HttpPost("refresh-token")]
    public async Task<ActionResult<LoginResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var storedToken = await _db.RefreshTokens
            .Include(rt => rt.Usuario)
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);

        if (storedToken == null)
            return Unauthorized(new { status = "Error", message = "Token inválido." });

        if (!storedToken.IsActive)
            return Unauthorized(new { status = "Error", message = "Token expirado o revocado." });

        // Revocar token usado (Rotación)
        storedToken.Revoked = DateTime.UtcNow;
        
        // Generar nuevos tokens
        var newJwtToken = GenerateJwtToken(storedToken.Usuario!);
        var newRefreshToken = GenerateRefreshToken();

        var newRtEntity = new RefreshToken
        {
            UserId = storedToken.UserId,
            Token = newRefreshToken,
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow
        };

        _db.RefreshTokens.Add(newRtEntity);
        _db.RefreshTokens.Update(storedToken); // Guardar revocación
        await _db.SaveChangesAsync();

        // Obtener accesos nuevamente (opcional, o devolver lista vacía si el frontend ya la tiene)
        // Para consistencia con LoginResponse, devolvemos la lista vacía o la consultamos
        // Aquí devolveremos la lista vacía para ahorrar query, asumiendo que el frontend mantiene el estado
        // O mejor: consultamos para estar seguros de que sigue teniendo acceso.
         var accesos = await _db.UsuarioEmpresas
            .Include(ue => ue.Empresa)
            .Include(ue => ue.Rol)
            .Where(ue => ue.UserId == storedToken.UserId && ue.UeActivo == 1)
            .Select(ue => new EmpresaAccess(
                ue.EmpSchema, 
                ue.Empresa!.EmpNombre, 
                ue.Rol!.RolDescripcion, 
                ue.UePredeterminado == 1
            ))
            .ToListAsync();

        return Ok(new LoginResponse(storedToken.UserId, storedToken.Usuario!.UserNombre ?? "", newJwtToken, newRefreshToken, accesos));
    }

    private string GenerateJwtToken(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.UserId),
                new Claim(ClaimTypes.Name, usuario.UserNombre ?? ""),
                new Claim(ClaimTypes.Email, usuario.UserEmail)
            }),
            Expires = DateTime.UtcNow.AddHours(4),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}

