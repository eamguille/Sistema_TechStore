using System;
using System.Collections.Generic;

namespace Techstore_WebApp.Models.DB;

public partial class Modelo
{
    public int IdModelo { get; set; }

    public string Modelo1 { get; set; } = null!;

    public int IdMarca { get; set; }

    public virtual Marca IdMarcaNavigation { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
