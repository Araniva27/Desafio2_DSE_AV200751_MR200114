using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APIUsuarios.Models
{
    public class UsuarioT
    {
        [Key]
        public int UsuarioId { get; set; }

        //[Required]
        //[StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres.")]
        public string Nombre { get; set; }

        //[Required]
        //[EmailAddress(ErrorMessage = "El email debe tener un formato válido.")]
        public string Email { get; set; }

        //[Required]
        //[MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string Contraseña { get; set; }
        
        [ForeignKey("Rol")]
        public int RolId { get; set; }
        
        [JsonIgnore]
        public virtual RolT? Rol { get; set; }
    }
}
