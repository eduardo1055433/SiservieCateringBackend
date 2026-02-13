using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiservieCatering.API.Data;
using SiservieCatering.API.Models.Tablas.General;

namespace SiservieCatering.API.Controllers.General;

public record LoginRequest(string UserId, string Password);
public record LoginResponse(string UserId, string Nombre, List<EmpresaAccess> Empresas);
public record EmpresaAccess(string Schema, string NombreEmpresa, string Rol, bool Predeterminado);

[ApiController]
[Route("api/general/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;

    public AuthController(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Inicia sesión y devuelve las empresas a las que el usuario tiene acceso.
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

        return Ok(new LoginResponse(usuario.UserId, usuario.UserNombre ?? "", accesos));
    }

    /// <summary>
    /// Registra un nuevo usuario (Solo para propósitos de prueba/inicialización).
    /// </summary>
    [HttpPost("register-demo")]
    public async Task<IActionResult> RegisterDemo(string userId, string password, string nombre, string email)
    {
        if (await _db.Usuarios.AnyAsync(u => u.UserId == userId))
            return BadRequest("El usuario ya existe.");

        var nuevoUsuario = new Usuario
        {
            UserId = userId,
            UserEmail = email,
            UserNombre = nombre,
            UserPassword = BCrypt.Net.BCrypt.HashPassword(password), // Hash seguro
            UserActivo = 1
        };

        _db.Usuarios.Add(nuevoUsuario);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Usuario creado exitosamente.", userId });
    }

    /// <summary>
    /// ENDPOINT TEMPORAL: Resetea el password del admin a '123456' usando la librería interna.
    /// </summary>
    [HttpGet("reset-admin")]
    public async Task<IActionResult> ResetAdmin()
    {
        var admin = await _db.Usuarios.FirstOrDefaultAsync(u => u.UserId == "admin");
        if (admin == null) return NotFound("Usuario admin no encontrado");

        // Generar hash usando la MISMA librería que verfica
        admin.UserPassword = BCrypt.Net.BCrypt.HashPassword("123456");
        await _db.SaveChangesAsync();

        return Ok(new { message = "Password de admin reseteado a '123456'", newHash = admin.UserPassword });
    }
}

