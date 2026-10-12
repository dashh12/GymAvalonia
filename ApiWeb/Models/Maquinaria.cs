using ApiWeb.Models;

namespace ApiWeb.Models
{
    public class Maquinaria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public DateTime FechaUltimoMantenimiento { get; set; }
    }
}