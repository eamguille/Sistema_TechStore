using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Techstore_WebApp.Models.DB;

public partial class Usuario
{
    [Key]
    public string IdUsuario { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]

    public string Nombres { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]

    public string Apellidos { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]

    public string NombreUsuario { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]

    public string Clave { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]

    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]

    public string Telefono { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]

    public string Direccion { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]

    public string Rol { get; set; } = null!;
    [Required(ErrorMessage = "Campo requerido")]

    public virtual ICollection<ComprasEmpresa> ComprasEmpresas { get; set; } = new List<ComprasEmpresa>();

    public virtual ICollection<MovimientosInventario> MovimientosInventarios { get; set; } = new List<MovimientosInventario>();

    public virtual ICollection<VentasEmpresa> VentasEmpresas { get; set; } = new List<VentasEmpresa>();
}
