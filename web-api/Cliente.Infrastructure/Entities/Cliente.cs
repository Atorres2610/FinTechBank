using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Cliente.Infrastructure.Entities
{
    public class Cliente : BaseEntitty
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}")]
        public required string Apellido { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}")]
        public required string NumeroCuenta { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Precision(18, 2)]
        public decimal Saldo { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(200, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}")]
        public required string Direccion { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}")]
        [Phone(ErrorMessage = "El formato del número telefónico es incorrecto.")]
        public required string Telefono { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}")]
        [EmailAddress(ErrorMessage = "El formato del correo es incorrecto. Ejemplo usuario@dominio.com")]
        public required string Correo { get; set; }

        [ForeignKey(nameof(Entities.TipoCliente))]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int IdTipoCliente { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}")]
        public required string EstadoCivil { get; set; }

        [StringLength(50, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public required string NumeroIdentificacion { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}")]
        public required string ProfesionOcupacion { get; set; }

        [ForeignKey(nameof(Entities.Genero))]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int IdGenero { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}")]
        public required string Nacionalidad { get; set; }

        [JsonIgnore]
        public virtual TipoCliente TipoCliente { get; set; } = null!;

        [JsonIgnore]
        public virtual Genero Genero { get; set; } = null!;
    }
}