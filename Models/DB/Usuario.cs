using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Techstore_WebApp.Models.DB;

public partial class Usuario
{
    public string IdUsuario { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string NombreUsuario { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public virtual ICollection<ComprasEmpresa> ComprasEmpresas { get; set; } = new List<ComprasEmpresa>();

    public virtual ICollection<MovimientosInventario> MovimientosInventarios { get; set; } = new List<MovimientosInventario>();

    public virtual ICollection<VentasEmpresa> VentasEmpresas { get; set; } = new List<VentasEmpresa>();
}
