using static WebApiAviones.Validations.PrimeraLetraMayuscula;

namespace WebApiAviones.Entidades
{
    public class Clase
    {
        public int Id { get; set; }

        [PrimeraLetraMayuscula]
        public string Name { get; set; }

        public int AvionId { get; set; }
        public Avion Avion { get; set; }
    }
}
