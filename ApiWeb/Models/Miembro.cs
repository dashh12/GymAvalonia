using System.ComponentModel.DataAnnotations;

namespace ApiWeb.Models
{
    public class Miembro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; } = "";

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
        public string Email { get; set; } = "";

        [Range(1, int.MaxValue, ErrorMessage = "Debe indicar una membresía válida")]
        public int MembresiaId { get; set; }
        public Membresia? Membresia { get; set; }

        public int? RutinaId { get; set; }
        public Rutina? Rutina { get; set; }

        public ICollection<Pago>? Pagos { get; set; }
    }
}
