using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Tiquetes.Models
{
    public class Tiquete
    {
        public int Id { get; set; }

        public DateTime Ingreso { get; set; }

        public DateTime Salida { get; set; }

        public string? Placa { get; set; }

        public Double Venta { get; set; }

        public string? Estado { get; set; }

        public int IdParqueo { get; set; }
    }
}
