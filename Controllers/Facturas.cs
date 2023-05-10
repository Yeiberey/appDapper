using appDapper2;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using appDapper.Controllers.Entidades;

namespace webApi.Controllers
{

    [ApiController]
    [Route("api/facturas")]
    public class FacturasController : ControllerBase
    {
        private readonly DbService conn;
        public FacturasController(DbService connection)
        {
            this.conn = connection;
        }
        [HttpGet("{noDocumento}")]
        public IActionResult get(string noDocumento)
        {
            using (IDbConnection db = conn.Connection)
            {
                var factura = db.QueryFirstOrDefault<Factura>(
                    @"SELECT p.NoIdentificacion, p.NombreCompleto, p.PrimerNombre, p.SegundoNombre, p.PrimerApellido, p.SegundoApellido, p.Direccion, 
                    d.Descripcion AS Departamento, c.Descripcion AS Ciudad, b.Descripcion AS Barrio
                    FROM SalidasDeMercanciaTercero p 
                    LEFT JOIN departamentos d ON p.DepartamentoId = d.DepartamentoId 
                    LEFT JOIN Ciudad c ON p.CiudadId = c.CiudadId 
                    LEFT JOIN Barrios b ON p.BarrioId = b.BarrioId
                    WHERE NoDocumento=@NoDocumento
                    ", new { NoDocumento = noDocumento });
                if (factura == null)
                {
                    return NotFound();
                }
                var productos = db.Query<Producto>(
                    @"SELECT ProductoId, DescripcionLarga, PrecioPublico AS PrecioUnitario, CantidadSalida, TotalRegistro AS Total, TarifaIvaVenta, EsBolsa, EquipoId
                    FROM SalidasDeMercanciaDetalle
                    WHERE NoDocumento = @NoDocumento",
                    new { NoDocumento = noDocumento }).AsList();

                factura.Productos = productos;
                return Ok(factura);
            }
        }
    }
}