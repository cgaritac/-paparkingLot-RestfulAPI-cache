using Empleados.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Tiquetes.Models;
using Parqueos.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tiquetes.Controllers
{
    [Route("api/Tiquetes")]
    [ApiController]
    public class TiquetesController : ControllerBase
    {
        // Caché
        private readonly IMemoryCache ElCache;

        public TiquetesController(IMemoryCache elCache)
        {
            ElCache = elCache;
        }

        [NonAction]
        private List<Models.Tiquete> ObtenerTiquetes()
        {
            List<Models.Tiquete> resultado;

            if (EstaVacioElCache())
            {
                resultado = new List<Models.Tiquete>();
                ElCache.Set("ListaDeTiquetes", resultado);
            }
            else
                resultado = (List<Models.Tiquete>)ElCache.Get("ListaDeTiquetes");

            return resultado;
        }

        [NonAction]
        private Models.Tiquete ObtenerElTiquete(int id)
        {
            List<Models.Tiquete> laLista;
            laLista = ObtenerTiquetes();

            foreach (Models.Tiquete tiquete in laLista)
            {
                if (tiquete.Id == id)
                    return tiquete;
            }

            return null;
        }

        [NonAction]
        private bool EstaVacioElCache()
        {
            if (ElCache.Get("ListaDeTiquetes") is null)
                return true;
            else return false;
        }



        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        [NonAction]
        public static bool EsEntero(double numero)
        {
            // Comprueba si el número es igual a su versión redondeada al entero más cercano
            return numero == Math.Round(numero);
        }

        [NonAction]
        public static bool EsFraccionMayorA0_5(double numero)
        {
            if (!EsEntero(numero)) // Verifica si no es un número entero
            {
                // Obtiene la parte decimal del número
                double parteDecimal = numero - Math.Floor(numero);

                // Comprueba si la parte decimal es mayor a 0.5
                return parteDecimal > 0.5;
            }

            return false; // El número es entero, no es una fracción mayor a 0.5
        }


        /// ////////////////////////////////////////////////////////////////////////////////////////////////



        // GET: api/<TiquetesController>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            if (ElCache.Get("ListaDeTiquetes") is null)
            {
                Models.Tiquete tiquete = new Models.Tiquete();
                List < Models.Tiquete> resultado = ObtenerTiquetes();

                tiquete.Id = 1;
                tiquete.Placa = "NCC171";
                tiquete.Ingreso = new DateTime(2023, 11, 6, 11, 0, 0);
                tiquete.Salida = new DateTime(2023, 11, 6, 12, 0, 0);
                tiquete.Venta = 1000;
                tiquete.Estado = "Cerrado";
                tiquete.IdParqueo = 1;

                resultado.Add(tiquete);

                ElCache.Set("ListaDeTiquetes", resultado);
            }

            return Ok(ObtenerTiquetes());
        }

        // POST api/<TiquetesController>
        [HttpPost("Create")]
        public ActionResult Create([FromBody] Models.Tiquete tiquete)
        {
            List<Models.Tiquete> resultado = ObtenerTiquetes();

            // Generar automáticamente el valor de "Id"
            int nextId = 1;

            if (resultado.Count > 0)
            {
                // Si hay tiquetes en la lista, obtener el valor del último "Id" y agregar 1
                nextId = resultado.Max(t => t.Id) + 1;
            }

            tiquete.Id = nextId;

            resultado.Add(tiquete);
            ElCache.Set("ListaDeTiquetes", resultado);

            return Ok();
        }

        //// GET api/<TiquetesController>/5
        [HttpGet("ObtenerTiquete/{id}")]
        public ActionResult<Models.Tiquete> ObtenerTiquete(int id)
        {
            return Ok(ObtenerElTiquete(id));
        }

        // PUT api/<TiquetesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Models.Tiquete tiquete)
        {
            List<Models.Tiquete> resultado = ObtenerTiquetes();
            List<Parqueo> listadoParqueos = (List<Parqueo>)ElCache.Get("ListaDeParqueos");
            int numeroParqueo = 0;

            var HorasEstacionado = tiquete.Salida.Subtract(tiquete.Ingreso).TotalHours;

            for (int i = 0; i < listadoParqueos.Count; i++)
            {
                if (listadoParqueos[i].Id == tiquete.IdParqueo)
                {
                    numeroParqueo = i; break;
                }
            }

            for (int i = 0; i < resultado.Count; i++)
            {
                if (resultado[i].Id == id)
                {
                    // Cerrar el tiquete
                    tiquete.Estado = "Cerrado";

                    // Establecer la tarifa por hora y la tarifa por media hora
                    double tarifaHora = Double.Parse(listadoParqueos[numeroParqueo].TarifaHora);
                    double tarifaMedia = Double.Parse(listadoParqueos[numeroParqueo].TarifaMedia);

                    double costo = 0.0;

                    if (HorasEstacionado <= 0.5)
                    {
                        costo = tarifaMedia;
                    }
                    else if (EsEntero(HorasEstacionado))
                    {
                        costo = tarifaHora * HorasEstacionado;
                    }
                    else if (EsFraccionMayorA0_5(HorasEstacionado))
                    {
                        costo = tarifaHora * (HorasEstacionado + 1);
                    }
                    else
                    {
                        costo = Math.Floor(HorasEstacionado) * tarifaHora + 1 * tarifaMedia;
                    }

                    tiquete.Venta = costo;
                    resultado.RemoveAt(i);
                    resultado.Add(tiquete);
                }
            }

            ElCache.Set("ListaDeTiquetes", resultado);
        }

        // DELETE api/<TiquetesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            List<Models.Tiquete> resultado = ObtenerTiquetes();

            for (int i = 0; i < resultado.Count; i++)
            {
                if (resultado[i].Id == id)
                    resultado.RemoveAt(i);
            }

            ElCache.Set("ListaDeTiquetes", resultado);
        }
    }
}
