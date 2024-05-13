using System.ComponentModel.DataAnnotations;

namespace Cliente.Infrastructure.Entities
{
    public class Genero : BaseEntitty
    {
        [StringLength(50, ErrorMessage = "La cantidad de caracteres no puede ser mayor a {1}.")]
        public required string Nombre { get; set; }

        public List<Cliente> Clientes { get; set; } = [];
    }
}