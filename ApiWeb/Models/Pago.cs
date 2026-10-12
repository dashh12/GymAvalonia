using ApiWeb.Models;

namespace ApiWeb.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public int MiembroId { get; set; }
        public Miembro? Miembro { get; set; }
    }
}