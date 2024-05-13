using System.ComponentModel.DataAnnotations;

namespace Cliente.Infrastructure.Entities
{
    public class Usuario : BaseEntitty
    {

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}")]
        [EmailAddress(ErrorMessage = "El formato del correo es incorrecto. Ejemplo usuario@dominio.com")]
        public required string Email { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(70, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}")]
        public required string Clave { get; set; }
    }
}
