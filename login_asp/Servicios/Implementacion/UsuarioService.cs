using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using login_asp.Models;
using login_asp.Servicios.Contrato;

namespace login_asp.Servicios.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly DbpruebaContext _dbContext;
        public UsuarioService(DbpruebaContext dbConext)
        {
            _dbContext = dbConext;
        }

        public async Task<Usuario> GetUsuarios(string correo, string clave)
        {
            Usuario usuario_encontrado = await _dbContext.Usuarios.Where(u => u.Correo == correo && u.Clave == clave)
            .FirstOrDefaultAsync();

            return usuario_encontrado;
        }

        public async Task<Usuario> SaveUsuarios(Usuario modelo)
        {
            _dbContext.Usuarios.Add(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }
        public async Task<bool> ActualizarUsuario(Usuario modelo)
        {
            try
            {
                _dbContext.Entry(modelo).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Manejo de errores, si es necesario
                return false;
            }
        }
        public async Task<Usuario> ObtenerUsuarioPorId(int id)
        {
            return await _dbContext.Usuarios.FindAsync(id);
        }

    }
}
