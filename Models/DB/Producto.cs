using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Techstore_WebApp.Models.DB;

public partial class Producto
{
    /// <summary>
    /// Identificador único del producto.
    /// </summary>
    public string IdProducto { get; set; } = null!; // Cambiado a string

    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    public string NombreProducto { get; set; } = null!;

    [Required(ErrorMessage = "La descripción del producto es obligatoria.")]
    public string DescripcionProducto { get; set; } = null!;

    [Required(ErrorMessage = "Debe seleccionar una categoría.")]
    public int IdCategoriaProducto { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un tipo.")]
    public int IdTipoProducto { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un modelo.")]
    public int IdModelo { get; set; }

    [Required(ErrorMessage = "El precio de compra es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser un valor mayor a 0.")]
    public decimal PrecioCompra { get; set; }

    [Required(ErrorMessage = "El precio de venta es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser un valor mayor a 0.")]
    public decimal PrecioVenta { get; set; }

    [Required(ErrorMessage = "La cantidad en stock es obligatoria.")]
    [Range(0, int.MaxValue, ErrorMessage = "La cantidad debe ser un número positivo.")]
    public int CantidadStock { get; set; }

    [Required(ErrorMessage = "Debe seleccionar el estado.")]
    public string Estado { get; set; } = null!;

    public virtual ICollection<DetallesCompra> DetallesCompras { get; set; } = new List<DetallesCompra>();

    public virtual ICollection<DetallesVenta> DetallesVenta { get; set; } = new List<DetallesVenta>();

    public virtual CategoriasProducto IdCategoriaProductoNavigation { get; set; } = null!;

    public virtual Modelo IdModeloNavigation { get; set; } = null!;

    public virtual TiposProducto IdTipoProductoNavigation { get; set; } = null!;

    public virtual ICollection<MovimientosInventario> MovimientosInventarios { get; set; } = new List<MovimientosInventario>();
}
