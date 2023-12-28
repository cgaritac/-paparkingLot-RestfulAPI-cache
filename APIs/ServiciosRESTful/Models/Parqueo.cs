using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Parqueos.Models
{
    public class Parqueo
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public int CantidadVehiculosMax { get; set; }

        public DateTime HoraApertura { get; set; }

        public DateTime HoraCierre { get; set; }

        public string TarifaHora { get; set; }

        public string TarifaMedia { get; set; }
    }
}
