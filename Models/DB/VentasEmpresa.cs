using System;
using System.Collections.Generic;

namespace Techstore_WebApp.Models.DB;

public partial class VentasEmpresa
{
    public int IdVenta { get; set; }

    public int IdCliente { get; set; }

    public string IdUsuario { get; set; } = null!;

    public DateOnly FechaVenta { get; set; }

    public decimal TotalVenta { get; set; }

    public string MetodoPago { get; set; } = null!;

    public string EstadoVenta { get; set; } = null!;

    public virtual ICollection<DetallesVenta> DetallesVenta { get; set; } = new List<DetallesVenta>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
