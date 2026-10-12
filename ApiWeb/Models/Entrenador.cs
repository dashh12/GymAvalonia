using ApiWeb.Models;

namespace ApiWeb.Models
{
    public class Entrenador
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Especialidad { get; set; }
    }
}