using System;
using System.Collections.Generic;

namespace Techstore_WebApp.Models.DB;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string NombreCliente { get; set; } = null!;

    public string Dui { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public virtual ICollection<VentasEmpresa> VentasEmpresas { get; set; } = new List<VentasEmpresa>();
}
