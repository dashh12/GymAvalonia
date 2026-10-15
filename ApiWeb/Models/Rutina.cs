using System.ComponentModel.DataAnnotations;

namespace ApiWeb.Models
{
    public class Rutina
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; } = "";

        [Required(ErrorMessage = "El nivel de dificultad es obligatorio")]
        public string NivelDificultad { get; set; } = "";

        [Range(1, int.MaxValue, ErrorMessage = "Debe indicar un entrenador válido")]
        public int EntrenadorId { get; set; }
        public Entrenador? Entrenador { get; set; }

        public ICollection<Miembro>? Miembros { get; set; }
    }
}
