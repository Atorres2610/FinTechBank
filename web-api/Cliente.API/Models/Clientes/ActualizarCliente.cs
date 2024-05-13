using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cliente.API.Models.Clientes
{
    public record ActualizarCliente(
            [Required(ErrorMessage = "El campo {0} es requerido")]
            int Id,

            [Required(ErrorMessage = "El campo {0} es requerido")]
            [StringLength(50, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}")]
            string Nombre,

            [Required(ErrorMessage = "El campo {0} es requerido")]
            [StringLength(50, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}")]
            string Apellido,

            [Required(ErrorMessage = "El campo {0} es requerido")]
            [StringLength(50, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}")]
            string NumeroCuenta,

            [Required(ErrorMessage = "El campo {0} es requerido")]
            decimal Saldo,

            [Required(ErrorMessage = "El campo {0} es requerido")]
            DateTime FechaNacimiento,

            [Required(ErrorMessage = "El campo {0} es requerido")]
            [StringLength(200, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}")]
            string Direccion,

            [Required(ErrorMessage = "El campo {0} es requerido")]
            [StringLength(50, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}")]
            [Phone(ErrorMessage = "El formato del número telefónico es incorrecto.")]
            string Telefono,

            [Required(ErrorMessage = "El campo {0} es requerido")]
            [StringLength(50, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}")]
            [EmailAddress(ErrorMessage = "El formato del correo es incorrecto. Ejemplo usuario@dominio.com")]
            string Correo,

            [Required(ErrorMessage = $"El campo {nameof(Infrastructure.Entities.TipoCliente)} es requerido")]
            int IdTipoCliente,

            [Required(ErrorMessage = "El campo {0} es requerido")]
            [StringLength(50, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}")]
            string EstadoCivil,

            [StringLength(50, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}")]
            [Required(ErrorMessage = "El campo {0} es requerido")]
            string NumeroIdentificacion,

            [Required(ErrorMessage = "El campo {0} es requerido")]
            [StringLength(50, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}")]
            string ProfesionOcupacion,

            [Required(ErrorMessage = $"El campo {nameof(Infrastructure.Entities.Genero)} es requerido")]
            int IdGenero,

            [Required(ErrorMessage = "El campo {0} es requerido")]
            [StringLength(50, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}")]
            string Nacionalidad
        )
    {
        public Infrastructure.Entities.Cliente GetToCliente()
        {
            return new()
            {
                Id = Id,
                Apellido = Apellido,
                Correo = Correo,
                Direccion = Direccion,
                EstadoCivil = EstadoCivil,
                Nacionalidad = Nacionalidad,
                Nombre = Nombre,
                NumeroCuenta = NumeroCuenta,
                NumeroIdentificacion = NumeroIdentificacion,
                ProfesionOcupacion = ProfesionOcupacion,
                Telefono = Telefono,
                FechaNacimiento = FechaNacimiento,
                IdGenero = IdGenero,
                IdTipoCliente = IdTipoCliente,
                Saldo = Saldo
            };
        }
    }
}