using System;
using System.Collections.Generic;

namespace login_asp.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? NombreUsuario { get; set; }

    public string? Apellido { get; set; }

    public string? Correo { get; set; }

    public string? Clave { get; set; }
}
