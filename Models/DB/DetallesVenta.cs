using System;
using System.Collections.Generic;

namespace Techstore_WebApp.Models.DB;

public partial class DetallesVenta
{
    public string IdDetalleVenta { get; set; } = null!;

    public int IdVenta { get; set; }

    public string IdProducto { get; set; } = null!;

    public int Cantidad { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual VentasEmpresa IdVentaNavigation { get; set; } = null!;
}
