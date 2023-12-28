using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Parqueos.Models;
using Tiquetes.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Reportes.Controllers
{
    [Route("api/Reportes")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        // Caché
        private readonly IMemoryCache ElCache;

        public ReportesController(IMemoryCache elCache)
        {
            ElCache = elCache;
        }

        [NonAction]
        private List<Tiquete> ObtenerTiquetes()
        {
            List<Tiquete> resultado = (List<Tiquete>)ElCache.Get("ListaDeTiquetes");

            return resultado;
        }


        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        // GET api/<ReportesController>/5
        [HttpGet("ObtenerVentasMes/{idParqueo}/{mes}")]
        public ActionResult<Double> ObtenerVentasMes(int idParqueo, string mes)
        {
            int numeroMes = 0;
            double ventasMes = 0;
            List<Tiquete> tiquetes = ObtenerTiquetes();

            switch (mes.ToLower())
            {
                case "enero":
                    numeroMes = 1;
                    break;
                case "febrero":
                    numeroMes = 2;
                    break;
                case "marzo":
                    numeroMes = 3;
                    break;
                case "abril":
                    numeroMes = 4;
                    break;
                case "mayo":
                    numeroMes = 5;
                    break;
                case "junio":
                    numeroMes = 6;
                    break;
                case "julio":
                    numeroMes = 7;
                    break;
                case "agosto":
                    numeroMes = 8;
                    break;
                case "septiembre":
                    numeroMes = 9;
                    break;
                case "octubre":
                    numeroMes = 10;
                    break;
                case "noviembre":
                    numeroMes = 11;
                    break;
                case "diciembre":
                    numeroMes = 12;
                    break;
            }

            foreach (var tiquete in tiquetes)
            {
                if (tiquete.Salida.Month == numeroMes && tiquete.IdParqueo == idParqueo)
                {
                    ventasMes += tiquete.Venta; // Suma las ventas si el mes coincide
                }
            }

            return ventasMes;
        }

        // GET api/<ReportesController>/5
        [HttpGet("ObtenerVentasDia/{idParqueo}/{salida}")]
        public ActionResult<Double> ObtenerVentasDia(int idParqueo, DateTime salida)
        {
            double ventasDia = 0;
            List<Tiquete> tiquetes = ObtenerTiquetes();

            foreach (var tiquete in tiquetes)
            {
                if (tiquete.Salida.Date == salida.Date && tiquete.IdParqueo == idParqueo)
                {
                    ventasDia += tiquete.Venta; // Suma las ventas si el dia coincide
                }
            }

            return ventasDia;
        }













        // POST api/<ReportesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ReportesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ReportesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
