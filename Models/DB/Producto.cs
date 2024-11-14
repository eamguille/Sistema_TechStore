using System;
using System.Collections.Generic;

namespace Techstore_WebApp.Models.DB;

public partial class Producto
{
    public string IdProducto { get; set; } = null!;

    public string NombreProducto { get; set; } = null!;

    public string DescripcionProducto { get; set; } = null!;

    public int IdCategoriaProducto { get; set; }

    public int IdTipoProducto { get; set; }

    public int IdModelo { get; set; }

    public decimal PrecioCompra { get; set; }

    public decimal PrecioVenta { get; set; }

    public int CantidadStock { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<DetallesCompra> DetallesCompras { get; set; } = new List<DetallesCompra>();

    public virtual ICollection<DetallesVenta> DetallesVenta { get; set; } = new List<DetallesVenta>();

    public virtual CategoriasProducto IdCategoriaProductoNavigation { get; set; } = null!;

    public virtual Modelo IdModeloNavigation { get; set; } = null!;

    public virtual TiposProducto IdTipoProductoNavigation { get; set; } = null!;

    public virtual ICollection<MovimientosInventario> MovimientosInventarios { get; set; } = new List<MovimientosInventario>();
}
