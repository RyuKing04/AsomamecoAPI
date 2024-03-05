using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AsomamecoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace AsomamecoAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly AsomamecoContext _dbContext;
    private readonly string _jwtSecret;

	public AuthController(AsomamecoContext dbContext)
	{
	    _dbContext = dbContext;
        _jwtSecret = "KGGK>HKHVHJVKBKJKJBKBKHKBMKHB";
	}

    [HttpPost("registro")]
    public IActionResult Registro(Usuario usuario)
    {
        // Verifica que el modelo de usuario sea válido
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Encripta la contraseña antes de registrar al usuario
        usuario.Contraseña = Encrypt(usuario.Contraseña);

        var rolPredeterminado = _dbContext.Rols.FirstOrDefault(r => r.Descripcion == "Ayudante");
        if (rolPredeterminado != null)
        {
            usuario.IdRol = rolPredeterminado.Id;
        }
        else
        {
            // Si no se encuentra el rol predeterminado, puedes manejarlo de alguna otra manera (por ejemplo, lanzar una excepción)
            return StatusCode(StatusCodes.Status500InternalServerError, "Error al asignar el rol predeterminado");
        }

        // Aquí guardas el usuario en tu base de datos
        _dbContext.Usuarios.Add(usuario);
        _dbContext.SaveChanges();

        return Ok("Usuario registrado exitosamente");
    }

    [HttpPost("login")]
    public IActionResult Login(LoginModel loginModel)
    {
        // Buscar el usuario por su correo en la base de datos
        var usuario = _dbContext.Usuarios.FirstOrDefault(u => u.Correo == loginModel.Correo);

        // Verificar si el usuario existe
        if (usuario == null)
        {
            return NotFound("Usuario no encontrado");
        }

        // Encriptar la contraseña proporcionada por el usuario
        var contraseñaEncriptada = Encrypt(loginModel.Contraseña);

        // Verificar si la contraseña encriptada coincide con la contraseña almacenada
        if (usuario.Contraseña != contraseñaEncriptada)
        {
            return BadRequest("Credenciales incorrectas");
        }

        // Autenticación exitosa, generar token JWT
        var token = GenerateJwtToken(usuario);

        // Devolver el token JWT en la respuesta
        return Ok(new { Token = token });
    }
    private string GenerateJwtToken(Usuario usuario)
    {
        // Configurar los claims del token (información del usuario)
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Correo)
                // Puedes agregar más claims según las necesidades de tu aplicación
            };

        // Configurar la clave secreta y los parámetros de seguridad del token
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddDays(7); // Duración del token (por ejemplo, 7 días)

        // Construir el token JWT
        var token = new JwtSecurityToken(
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        // Generar el token como string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpGet("roles")]
    public IActionResult ObtenerRoles()
    {
        var roles = _dbContext.Rols.ToList();
        return Ok(roles);
    }

    [HttpPost("asignar-rol")]
    public IActionResult AsignarRol(AgregarRolModel agregarRolModel)
    {
        var usuario = _dbContext.Usuarios.FirstOrDefault(u => u.Id == agregarRolModel.UsuarioId);
        if (usuario == null)
        {
            return NotFound("Usuario no encontrado");
        }

        var rol = _dbContext.Rols.FirstOrDefault(r => r.Id == agregarRolModel.RolId);
        if (rol == null)
        {
            return NotFound("Rol no encontrado");
        }

        usuario.IdRol = rol.Id;
        _dbContext.SaveChanges();

        return Ok("Rol asignado correctamente");
    }

    private static string Encrypt(string contraseña)
    {
        StringBuilder sb = new StringBuilder();
        using (SHA256 hash = SHA256Managed.Create())
        {
            Encoding enc = Encoding.UTF8;
            Byte[] result = hash.ComputeHash(enc.GetBytes(contraseña));
            foreach (Byte b in result)
                sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }

    [HttpGet("obtenerUsuarios")]
    public IActionResult ObtenerUsuarios()
    {
        var usuarios = _dbContext.Usuarios.ToList();
        return Ok(usuarios);
    }   
}

public class LoginModel
{
    public string Correo { get; set; }
    public string Contraseña { get; set; }
}

public class AgregarRolModel
{
    public int UsuarioId { get; set; }
    public int RolId { get; set; }
}

