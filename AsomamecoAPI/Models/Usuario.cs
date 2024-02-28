using System;
using System.Collections.Generic;

namespace AsomamecoAPI.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public int Telefono { get; set; }

    public int Cedula { get; set; }

    public string Contraseña { get; set; } = null!;

    public int IdRol { get; set; }

    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();

    public virtual Rol IdRolNavigation { get; set; } = null!;
}
