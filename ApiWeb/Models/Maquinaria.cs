using System.ComponentModel.DataAnnotations;

namespace ApiWeb.Models
{
    public class Maquinaria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "El músculo es obligatorio")]
        public string Musculo { get; set; } = "";

        [Required(ErrorMessage = "El estado es obligatorio")]
        public string Estado { get; set; } = "Operativa";

        public DateTime FechaUltimoMantenimiento { get; set; }
    }
}
