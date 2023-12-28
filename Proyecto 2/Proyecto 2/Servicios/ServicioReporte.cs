using Newtonsoft.Json;
using Proyecto_2.Models;

namespace Proyecto_2.Servicios
{
    public class ServicioReporte : IServicioReporte
    {
        private string _baseurl;

        public ServicioReporte()
        {

            _baseurl = "http://localhost:5135/";
        }

        public async Task<Double> ObtenerVentasMes(int idParqueo, string mes)
        {
            Double VentasDelMes = 1;
            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseurl);
            var response = await cliente.GetAsync($"api/Reportes/ObtenerVentasMes/{idParqueo}/{mes}");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<Double>(json_respuesta);
                VentasDelMes = resultado;
            }
            return VentasDelMes;
        }

        public async Task<Double> ObtenerVentasDia(int idParqueo, DateTime salida)
        {
            Double VentasDelDia = 1;
            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(_baseurl);
            var response = await cliente.GetAsync($"api/Reportes/ObtenerVentasDia/{idParqueo}/{salida}");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<Double>(json_respuesta);
                VentasDelDia = resultado;
            }
            return VentasDelDia;
        }
    }
}
