using System;
using System.Collections.Generic;

namespace Techstore_WebApp.Models.DB;

public partial class ComprasEmpresa
{
    public int IdCompra { get; set; }

    public int IdProveedor { get; set; }

    public string IdUsuario { get; set; } = null!;

    public DateOnly FechaCompra { get; set; }

    public decimal TotalCompra { get; set; }

    public string EstadoCompra { get; set; } = null!;

    public virtual ICollection<DetallesCompra> DetallesCompras { get; set; } = new List<DetallesCompra>();

    public virtual Proveedore IdProveedorNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
