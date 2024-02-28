using System;
using System.Collections.Generic;

namespace AsomamecoAPI.Models;

public partial class Evento
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public string Fecha { get; set; } = null!;

    public string Lugar { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public bool Tiquete { get; set; }

    public int IdCateringService { get; set; }

    public int IdUsuario { get; set; }

    public virtual CateringService IdCateringServiceNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Asociado> IdAsociados { get; set; } = new List<Asociado>();
}
