using appDapper2;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using appDapper.Controllers.Entidades;
using appDapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace webApi.Controllers
{

    [ApiController]
    [Route("api/facturas")]
    public class FacturasController : ControllerBase
    {
        // Inyecta JwtService en tu controlador o servicio
        private readonly JWTService _jwtService;

        private readonly DbService conn;
        public FacturasController(DbService connection, JWTService jwtService)
        {
            this.conn = connection;
            this._jwtService = jwtService;
        }
        [HttpGet]
        [Authorize]
        public IActionResult getFactura(string type, string number)
        {

            /*var authorizationHeader = Request.Headers["Authorization"];
            var token = authorizationHeader.ToString().Replace("Bearer ", "");*/

            //var claimsPrincipal = _jwtService.ValidateToken(token);
            //if (claimsPrincipal == null)
            //{
            //    return Unauthorized();
            //}

            //token es válido
            if (type == "noIdentificacion")
            {
                using (IDbConnection db = conn.Connection)
                {
                    var facturas = db.Query<Factura>(
                        @"SELECT p.NoDocumento, p.NoIdentificacion, p.NombreCompleto, p.PrimerNombre, p.SegundoNombre, p.PrimerApellido, p.SegundoApellido, p.Direccion, 
                    d.Descripcion AS Departamento, c.Descripcion AS Ciudad, b.Descripcion AS Barrio
                    FROM SalidasDeMercanciaTercero p 
                    LEFT JOIN departamentos d ON p.DepartamentoId = d.DepartamentoId 
                    LEFT JOIN Ciudad c ON p.CiudadId = c.CiudadId 
                    LEFT JOIN Barrios b ON p.BarrioId = b.BarrioId
                    WHERE NoIdentificacion=@NoIdentificacion
                    ", new { NoIdentificacion = number }).AsList();
                    return Ok(facturas);
                }
            }
            
            if(type == "noDocumento")
            {

            using (IDbConnection db = conn.Connection)
            {
                var factura = db.QueryFirstOrDefault<Factura>(
                    @"SELECT p.NoDocumento, p.NoIdentificacion, p.NombreCompleto, p.PrimerNombre, p.SegundoNombre, p.PrimerApellido, p.SegundoApellido, p.Direccion, 
                    d.Descripcion AS Departamento, c.Descripcion AS Ciudad, b.Descripcion AS Barrio
                    FROM SalidasDeMercanciaTercero p 
                    LEFT JOIN departamentos d ON p.DepartamentoId = d.DepartamentoId 
                    LEFT JOIN Ciudad c ON p.CiudadId = c.CiudadId 
                    LEFT JOIN Barrios b ON p.BarrioId = b.BarrioId
                    WHERE NoDocumento=@NoDocumento
                    ", new { NoDocumento = number });
                if (factura == null)
                {
                    return NotFound();
                }
                var productos = db.Query<Producto>(
                    @"SELECT ProductoId, DescripcionLarga, PrecioPublico AS PrecioUnitario, CantidadSalida, TotalRegistro AS Total, TarifaIvaVenta, EsBolsa, EquipoId
                    FROM SalidasDeMercanciaDetalle
                    WHERE NoDocumento = @NoDocumento",
                    new { NoDocumento = number }).AsList();

                factura.Productos = productos;
                return Ok(factura);
            }
            }
            return NotFound();
        }

    }
}