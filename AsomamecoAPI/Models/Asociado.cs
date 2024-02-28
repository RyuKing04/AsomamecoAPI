using System;
using System.Collections.Generic;

namespace AsomamecoAPI.Models;

public partial class Asociado
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public int Cedula { get; set; }

    public bool Status { get; set; }

    public string Correo { get; set; } = null!;

    public int Telefono { get; set; }

    public virtual ICollection<Evento> IdEventos { get; set; } = new List<Evento>();
}
