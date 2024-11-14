using System;
using System.Collections.Generic;

namespace Techstore_WebApp.Models.DB;

public partial class DetallesCompra
{
    public string IdDetalleCompra { get; set; } = null!;

    public int IdCompra { get; set; }

    public string IdProducto { get; set; } = null!;

    public int Cantidad { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public virtual ComprasEmpresa IdCompraNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
