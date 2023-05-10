using appDapper2;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using appDapper.Controllers.Entidades;

namespace webApi.Controllers
{

    [ApiController]
    [Route("api/personas")]
    public class PersonasController : ControllerBase
    {
        private readonly DbService conn;
        public PersonasController(DbService connection)
        {
            this.conn = connection;
        }
        [HttpGet]
        public IActionResult get()
        {
            using (IDbConnection db = conn.Connection)
            {
                var SQL = @"";
                var personas = db.Query<Persona>(
                    @"SELECT TOP 100 p.PersonaId,p.NoIdentificacion, p.PrimerNombre, p.SegundoNombre, p.PrimerApellido, p.SegundoApellido, p.Direccion, p.Telefono1, 
                     d.Descripcion AS Departamento, c.Descripcion AS Ciudad, b.Descripcion AS Barrio
                    FROM personas p 
                    LEFT JOIN departamentos d ON p.DepartamentoId = d.DepartamentoId 
                    LEFT JOIN Ciudad c ON p.CiudadId = c.CiudadId 
                    INNER JOIN Barrios b ON p.BarrioId = b.BarrioId").ToList();
                return Ok(personas);
            }
        }
    }
}