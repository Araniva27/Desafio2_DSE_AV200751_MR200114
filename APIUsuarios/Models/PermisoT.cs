using System.ComponentModel.DataAnnotations;

namespace APIUsuarios.Models
{
    public class PermisoT
    {
        [Key]
        public int PermisoId { get; set; }

        //[Required]
        //[StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre del permiso debe tener entre 3 y 50 caracteres.")]
        public string Nombre { get; set; }

        public string? Descripcion { get; set; }
    }
}
