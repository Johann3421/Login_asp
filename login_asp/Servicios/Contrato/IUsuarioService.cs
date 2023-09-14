using Microsoft.EntityFrameworkCore;
using login_asp.Models;


namespace login_asp.Servicios.Contrato
{
    public interface IUsuarioService
    {
        Task<Usuario> GetUsuarios(string correo, string clave);

        Task<Usuario> SaveUsuarios(Usuario modelo);


    }
}
