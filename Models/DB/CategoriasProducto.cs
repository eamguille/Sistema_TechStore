using System;
using System.Collections.Generic;

namespace Techstore_WebApp.Models.DB;

public partial class CategoriasProducto
{
    public int IdCategoriaProducto { get; set; }

    public string Categoria { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
