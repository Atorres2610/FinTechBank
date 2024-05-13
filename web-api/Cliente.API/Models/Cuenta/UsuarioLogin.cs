using System.ComponentModel.DataAnnotations;

namespace Cliente.API.Models.Cuenta
{
    public record UsuarioLogin(
        [Required(ErrorMessage = "El campo {0} es requerido")] string Email,
        [Required(ErrorMessage = "El campo {0} es requerido")] string Clave)
    {
        public UsuarioInfo GetUsuarioInfo()
        {
            return new UsuarioInfo(Email, Clave);
        }
    }
}