using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Cliente.Infrastructure.Entities
{
    public class BaseEntitty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonIgnore]
        public DateTime FechaModificacion { get; set; } = DateTime.Now.ToUniversalTime();
    }
}