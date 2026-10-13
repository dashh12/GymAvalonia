namespace ApiWeb.Models
{
    public class Miembro
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Email { get; set; } = "";

        public int MembresiaId { get; set; }
        public Membresia? Membresia { get; set; }

        public int? RutinaId { get; set; }
        public Rutina? Rutina { get; set; }

        public ICollection<Pago>? Pagos { get; set; }
    }
}
