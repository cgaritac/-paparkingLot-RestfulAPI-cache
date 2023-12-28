using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Parqueos.Controllers
{
    [Route("api/Parqueos")]
    [ApiController]
    public class ParqueosController : ControllerBase
    {
        // Caché
        private readonly IMemoryCache ElCache;

        public ParqueosController(IMemoryCache elCache)
        {
            ElCache = elCache;
        }

        [NonAction]
        private List<Models.Parqueo> ObtenerParqueos()
        {
            List<Models.Parqueo> resultado;

            if (EstaVacioElCache())
            {
                resultado = new List<Models.Parqueo>();
                ElCache.Set("ListaDeParqueos", resultado);
            }
            else
                resultado = (List<Models.Parqueo>)ElCache.Get("ListaDeParqueos");

            return resultado;
        }

        [NonAction]
        private Models.Parqueo ObtenerElParqueo(int id)
        {
            List<Models.Parqueo> laLista;
            laLista = ObtenerParqueos();

            foreach (Models.Parqueo parqueo in laLista)
            {
                if (parqueo.Id == id)
                    return parqueo;
            }

            return null;
        }

        [NonAction]
        private bool EstaVacioElCache()
        {
            if (ElCache.Get("ListaDeParqueos") is null)
                return true;
            else return false;
        }



        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




        // GET: api/<ParqueosController>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            if (ElCache.Get("ListaDeParqueos") is null)
            {
                Models.Parqueo parqueo = new Models.Parqueo();
                List<Models.Parqueo> resultado = ObtenerParqueos();

                parqueo.Id = 1;
                parqueo.Nombre = "San Rafael";
                parqueo.HoraApertura = new DateTime(1, 1, 1, 5, 0, 0);
                parqueo.HoraCierre = new DateTime(1, 1, 1, 22, 0, 0);
                parqueo.CantidadVehiculosMax = 30;
                parqueo.TarifaHora = "1000";
                parqueo.TarifaMedia = "600";
               

                resultado.Add(parqueo);

                ElCache.Set("ListaDeParqueos", resultado);
            }

            return Ok(ObtenerParqueos());
        }

        // POST api/<ParqueosController>
        [HttpPost("Create")]
        public ActionResult Create([FromBody] Models.Parqueo parqueo)
        {
            List<Models.Parqueo> resultado = ObtenerParqueos();

            // Generar automáticamente el valor de "Id"
            int nextId = 1;

            if (resultado.Count > 0)
            {
                // Si hay parqueos en la lista, obtener el valor del último "Id" y agregar 1
                nextId = resultado.Max(t => t.Id) + 1;
            }

            parqueo.Id = nextId;

            resultado.Add(parqueo);

            ElCache.Set("ListaDeParqueos", resultado);

            return Ok();
        }

        //// GET api/<ParqueosController>/5
        [HttpGet("ObtenerParqueo/{id}")]
        public ActionResult<Models.Parqueo> ObtenerParqueo(int id)
        {
            return Ok(ObtenerElParqueo(id));
        }

        // PUT api/<ParqueosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Models.Parqueo parqueo)
        {
            List<Models.Parqueo> resultado = ObtenerParqueos();

            for (int i = 0; i < resultado.Count; i++)
            {
                if (resultado[i].Id == id)
                {
                    resultado.RemoveAt(i);
                    resultado.Add(parqueo);
                }
            }

            ElCache.Set("ListaDeParqueos", resultado);
        }

        // DELETE api/<ParqueosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            List<Models.Parqueo> resultado = ObtenerParqueos();

            for (int i = 0; i < resultado.Count; i++)
            {
                if (resultado[i].Id == id)
                    resultado.RemoveAt(i);
            }

            ElCache.Set("ListaDeParqueos", resultado);
        }
    }
}
