using Empleados.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Parqueos.Models;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Empleados.Controllers
{
    [Route("api/Empleados")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        // Caché
        private readonly IMemoryCache ElCache;

        public EmpleadosController(IMemoryCache elCache)
        {
            ElCache = elCache;
        }

        [NonAction]
        private List<Models.Empleado> ObtenerEmpleados()
        {
            List<Models.Empleado> resultado;

            if (EstaVacioElCache())
            {
                resultado = new List<Models.Empleado>();
                ElCache.Set("ListaDeEmpleados", resultado);
            }
            else
                resultado = (List<Models.Empleado>)ElCache.Get("ListaDeEmpleados");

            return resultado;
        }

        [NonAction]
        private Models.Empleado ObtenerElEmpleado(int id)
        {
            List<Models.Empleado> laLista;
            laLista = ObtenerEmpleados();

            foreach (Models.Empleado empleado in laLista)
            {
                if (empleado.Id == id)
                    return empleado;
            }

            return null;
        }

        [NonAction]
        private bool EstaVacioElCache()
        {
            if (ElCache.Get("ListaDeEmpleados") is null)
                return true;
            else return false;
        }


        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        // GET: api/<EmpleadosController>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            if (ElCache.Get("ListaDeEmpleados") is null)
            {
                Models.Empleado empleado = new Models.Empleado();
                List<Models.Empleado> resultado = ObtenerEmpleados();

                empleado.Id = 1;
                empleado.Telefono = "71098984";
                empleado.Email = "cgaritac@gmai.com";
                empleado.NumeroCedula = "0206050530";
                empleado.Nombre = "Carlos";
                empleado.Direccion = "Pavas";
                empleado.TipoContacto = "Esposa";
                empleado.PrimerApellido = "Garita";
                empleado.SegundoApellido = "Campos";
                empleado.FechaIngreso = new DateTime(2023, 11, 9);
                empleado.FechaNacimiento = new DateTime(1985, 03, 30);
                empleado.IdParqueo = 1;

                resultado.Add(empleado);
               
                ElCache.Set("ListaDeEmpleados", resultado);
            }

            return Ok(ObtenerEmpleados());
        }

        // POST api/<EmpleadosController>
        [HttpPost("Create")]
        public ActionResult Create([FromBody] Models.Empleado empleado)
        {
            List<Models.Empleado> resultado = ObtenerEmpleados();

            // Generar automáticamente el valor de "Id"
            int nextId = 1;

            if (resultado.Count > 0)
            {
                // Si hay empleados en la lista, obtener el valor del último "Id" y agregar 1
                nextId = resultado.Max(t => t.Id) + 1;
            }

            empleado.Id = nextId;

            resultado.Add(empleado);
            ElCache.Set("ListaDeEmpleados", resultado);

            return Ok();
        }

        //// GET api/<EmpleadosController>/5
        [HttpGet("ObtenerEmpleado/{id}")]
        public ActionResult<Empleado> ObtenerEmpleado(int id)
        {
            return Ok(ObtenerElEmpleado(id));
        }

        // PUT api/<EmpleadosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Models.Empleado empleado)
        {
            List<Models.Empleado> resultado = ObtenerEmpleados();

            for (int i = 0; i < resultado.Count; i++)
            {
                if (resultado[i].Id == id)
                {
                    resultado.RemoveAt(i);
                    resultado.Add(empleado);
                }
            }

            ElCache.Set("ListaDeEmpleados", resultado);
        }

        // DELETE api/<EmpleadosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            List<Models.Empleado> resultado = ObtenerEmpleados();

            for (int i = 0; i < resultado.Count; i++)
            {
                if (resultado[i].Id == id)
                    resultado.RemoveAt(i);
            }

            ElCache.Set("ListaDeEmpleados", resultado);
        }
    }
}
