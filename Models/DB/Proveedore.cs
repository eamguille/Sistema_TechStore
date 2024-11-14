using System;
using System.Collections.Generic;

namespace Techstore_WebApp.Models.DB;

public partial class Proveedore
{
    public int IdProveedor { get; set; }

    public string NombreProveedor { get; set; } = null!;

    public string Dui { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public virtual ICollection<ComprasEmpresa> ComprasEmpresas { get; set; } = new List<ComprasEmpresa>();
}
