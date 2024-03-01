using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AsomamecoAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AsomamecoAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly AsomamecoContext _dbContext;

	public AuthController(AsomamecoContext dbContext)
	{
		_dbContext = dbContext;
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
        // Encripta la contraseña proporcionada por el usuario antes de verificarla
        var contraseñaEncriptada = Encrypt(loginModel.Contraseña);

        // Aquí realizas la lógica de autenticación

        return Ok("Inicio de sesión exitoso");
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

