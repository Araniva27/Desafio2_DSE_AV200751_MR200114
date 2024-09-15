using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APIUsuarios.Models
{
    public class RolT
    {
        [Key]
        public int RolId { get; set; }

        //[Required]
        //[StringLength(30, MinimumLength = 3, ErrorMessage = "El nombre del rol debe tener entre 3 y 30 caracteres.")]
        public string Nombre { get; set; }

        public string? Descripcion { get; set; }

        [JsonIgnore]
        public ICollection<UsuarioT>? Usuarios { get; set; } 
    }
}
