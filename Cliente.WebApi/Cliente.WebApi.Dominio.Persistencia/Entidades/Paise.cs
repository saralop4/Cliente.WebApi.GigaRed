using System;
using System.Collections.Generic;

namespace Cliente.WebApi.Dominio.Persistencia.Entidades;

public partial class Paise
{
    public long IdPais { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Ciudade> Ciudades { get; set; } = new List<Ciudade>();
}
