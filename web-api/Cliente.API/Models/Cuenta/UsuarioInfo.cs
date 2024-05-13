using Cliente.API.Helpers;
using Cliente.Infrastructure.Entities;
using System.ComponentModel.DataAnnotations;

namespace Cliente.API.Models.Cuenta
{
    public record UsuarioInfo(
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "El formato del correo es incorrecto. Ejemplo usuario@dominio.com")]
        string Email,

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MinLength(7, ErrorMessage = "La clave debe tener más de {1} carateres.")]
        string Clave)
    {
        public Usuario GetToUsuario()
        {
            return new() { Clave = Clave.EncryptSha256(), Email = Email };
        }
    }
}