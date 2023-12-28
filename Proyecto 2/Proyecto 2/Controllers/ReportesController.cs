using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_2.Models;
using Proyecto_2.Servicios;

namespace Proyecto_2.Controllers
{
    public class ReportesController : Controller
    {
        private readonly IServicioReporte _iservicioReporte;
        private readonly IServicioTiquete _iservicioTiquete;

        public ReportesController(IServicioReporte iservicioReporte, IServicioTiquete iservicioTiquete)
        {
            _iservicioReporte = iservicioReporte;
            _iservicioTiquete = iservicioTiquete;
        }


        /// ////////////////////////////////////////////////////////////////////////////////////////////////


        // GET: ReportesController
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // GET: ReportesController/Details/5
        public ActionResult Details(int idParqueo, string mes)
        {
            return View();
        }

        // GET: ReportesController/TopParqueos
        public ActionResult TopParqueos(string mes)
        {
            //ListaTopParqueos(mes);

            return View();
        }

        // GET: ReportesController/VentasMes
        public async Task<ActionResult> VentasMes(int idParqueo, string mes)
        {
            if (idParqueo != 0 && mes != null)
            {
                Double ventasdelMes = await _iservicioReporte.ObtenerVentasMes(idParqueo, mes);

                Models.Reporte nuevoReporte = new Models.Reporte
                {
                    Venta = ventasdelMes,
                    Mes = mes,
                    IdParqueo = idParqueo
                };

                return View(nuevoReporte);
            }

            else return RedirectToAction(nameof(Index));
        }

        // GET: ReportesController/VentasDia
        public async Task<ActionResult> VentasDia(DateTime salida, int idParqueo)
        {
            if (idParqueo != 0)
            {
                Double ventasdelDia = await _iservicioReporte.ObtenerVentasDia(idParqueo, salida);

                Models.Reporte nuevoReporte = new Models.Reporte
                {
                    Venta = ventasdelDia,
                    Salida = salida,
                    IdParqueo = idParqueo
                };

                return View(nuevoReporte);
            }

            else return RedirectToAction(nameof(Index));
        }
    }
        
}
