using System.ComponentModel.DataAnnotations;

namespace ApiWeb.Models
{
    public class Pago
    {
        public int Id { get; set; }

        public DateTime FechaPago { get; set; }

        [Range(0.01, 100000000, ErrorMessage = "El monto debe ser mayor a 0")]
        public decimal Monto { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe indicar un miembro válido")]
        public int MiembroId { get; set; }
        public Miembro? Miembro { get; set; }
    }
}
