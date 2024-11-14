using System;
using System.Collections.Generic;

namespace Techstore_WebApp.Models.DB;

public partial class MovimientosInventario
{
    public int IdMovimiento { get; set; }

    public string IdProducto { get; set; } = null!;

    public string IdUsuario { get; set; } = null!;

    public string TipoMovimiento { get; set; } = null!;

    public int Cantidad { get; set; }

    public DateOnly FechaMovimiento { get; set; }

    public string DescripcionMovimiento { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
