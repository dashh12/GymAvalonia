namespace ApiWeb.Models
{
    public class Maquinaria
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Musculo { get; set; } = "";
        public string Estado { get; set; } = "Operativa";
        public DateTime FechaUltimoMantenimiento { get; set; }
    }
}
