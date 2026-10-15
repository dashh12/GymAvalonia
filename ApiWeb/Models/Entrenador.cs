using System.ComponentModel.DataAnnotations;

namespace ApiWeb.Models
{
    public class Entrenador
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; } = "";

        [Required(ErrorMessage = "La especialidad es obligatoria")]
        public string Especialidad { get; set; } = "";

        public ICollection<Rutina>? Rutinas { get; set; }
    }
}
