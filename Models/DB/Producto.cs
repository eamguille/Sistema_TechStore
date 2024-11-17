using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Techstore_WebApp.Models.DB;

public partial class Producto
{
    [Required(ErrorMessage = "El ID del producto es obligatorio.")]
    public string IdProducto { get; set; } = null!;

    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre del producto no puede exceder los 100 caracteres.")]
    public string NombreProducto { get; set; } = null!;

    [Required(ErrorMessage = "La descripción del producto es obligatoria.")]
    public string DescripcionProducto { get; set; } = null!;

    [Required(ErrorMessage = "La categoría del producto es obligatoria.")]
    public int IdCategoriaProducto { get; set; }

    [Required(ErrorMessage = "El tipo de producto es obligatorio.")]
    public int IdTipoProducto { get; set; }

    [Required(ErrorMessage = "El modelo del producto es obligatorio.")]
    public int IdModelo { get; set; }

    [Required(ErrorMessage = "El precio de compra es obligatorio.")]
    [Range(0, double.MaxValue, ErrorMessage = "El precio de compra debe ser un valor positivo.")]
    public decimal PrecioCompra { get; set; }

    [Required(ErrorMessage = "El precio de venta es obligatorio.")]
    [Range(0, double.MaxValue, ErrorMessage = "El precio de venta debe ser un valor positivo.")]
    public decimal PrecioVenta { get; set; }

    [Required(ErrorMessage = "La cantidad en stock es obligatoria.")]
    [Range(0, int.MaxValue, ErrorMessage = "La cantidad en stock debe ser un valor positivo.")]
    public int CantidadStock { get; set; }

    [Required(ErrorMessage = "El estado es obligatorio.")]
    [StringLength(50, ErrorMessage = "El estado no puede exceder los 50 caracteres.")]
    public string Estado { get; set; } = null!;

    public virtual ICollection<DetallesCompra> DetallesCompras { get; set; } = new List<DetallesCompra>();

    public virtual ICollection<DetallesVenta> DetallesVenta { get; set; } = new List<DetallesVenta>();

    public virtual CategoriasProducto IdCategoriaProductoNavigation { get; set; } = null!;

    public virtual Modelo IdModeloNavigation { get; set; } = null!;

    public virtual TiposProducto IdTipoProductoNavigation { get; set; } = null!;

    public virtual ICollection<MovimientosInventario> MovimientosInventarios { get; set; } = new List<MovimientosInventario>();
}
