using Proyecto_2.Models;

namespace Proyecto_2.Servicios
{
    public interface IServicioReporte
    {
        public Task<Double> ObtenerVentasMes(int idParqueo, string mes);

        public Task<Double> ObtenerVentasDia(int idParqueo, DateTime salida);
    }
}
