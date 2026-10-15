using System.ComponentModel.DataAnnotations;

namespace ApiWeb.Models
{
    public class Membresia
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El tipo es obligatorio")]
        public string Tipo { get; set; } = "";

        [Range(0.01, 100000000, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        public ICollection<Miembro>? Miembros { get; set; }
    }
}
