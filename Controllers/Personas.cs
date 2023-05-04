using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using webApi.Controllers.Entidades;

namespace webApi.Controllers
{

    [ApiController]
    [Route("api/personas")]
    public class PersonasController : ControllerBase
    {
        public PersonasController(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        [HttpGet]
        public IActionResult get()
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("defaultConnection")))
            {
                db.Open();
                var personas = db.Query<Persona>(
                    "SELECT " +
                    "p.PersonaId,p.NoIdentificacion, p.PrimerNombre, p.SegundoNombre, p.PrimerApellido, p.SegundoApellido, p.Direccion, p.Telefono1, " +
                    "d.Descripcion AS Departamento, c.Descripcion AS Ciudad, b.Descripcion AS Barrio " +
                    "FROM personas p " +
                    "LEFT JOIN departamentos d ON p.DepartamentoId = d.DepartamentoId " +
                    "LEFT JOIN Ciudad c ON p.CiudadId = c.CiudadId " +
                    "LEFT JOIN Barrios b ON p.BarrioId = b.BarrioId").ToList();
                return Ok(personas);
            }
        }
    }
}