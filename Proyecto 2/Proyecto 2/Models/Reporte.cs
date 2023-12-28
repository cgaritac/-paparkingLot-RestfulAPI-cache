using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Proyecto_2.Models
{
    public class Reporte
    {
        [DisplayName("Fecha y hora de ingreso")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo es requerido.")]
        public DateTime Ingreso { get; set; }

        [DisplayName("Fecha y hora de salida")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo es requerido.")]
        public DateTime Salida { get; set; }

        [DisplayName("Mes")]
        public string? Mes { get; set; }

        [DisplayName("Monto total (\t₡ )")]
        public Double Venta { get; set; }

        [DisplayName("Posición")]
        public int Posicion { get; set; }

        [DisplayName("N° Parqueo")]
        public int IdParqueo { get; set; }
    }
}
