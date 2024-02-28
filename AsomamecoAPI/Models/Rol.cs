using System;
using System.Collections.Generic;

namespace AsomamecoAPI.Models;

public partial class Rol
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
