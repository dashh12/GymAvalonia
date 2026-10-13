namespace ApiWeb.Models
{
    public class Membresia
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = "";
        public decimal Precio { get; set; }

        public ICollection<Miembro>? Miembros { get; set; }
    }
}
