﻿using appDapper2;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace appDapper.Controllers
{
    public class UserCredentials
    {
        public string Username { get; set; }
        //public string Password { get; set; }
    }
    class User
    {
        private readonly DbService conn;

        public User(DbService connection)
        {
            this.conn = connection;
        }
        public string Authenticate(string username/*, string password*/)
        {
            using (IDbConnection db = conn.Connection)
            {
                // Realiza la consulta para verificar las credenciales en la base de datos utilizando Dapper
                string query = "SELECT SalidaId FROM SalidasDeMercanciaTercero WHERE PrimerNombre = @Username";
                var parameters = new { Username = username/*, Password = password */};
                string userId = db.QueryFirstOrDefault<string>(query, parameters);

                return userId;
            }
        }

    }

    [ApiController]
    [Route("/api/")]
    public class CuentasController : ControllerBase
    {

        // Inyecta JwtService en tu controlador o servicio
        private readonly JWTService _jwtService;
        private readonly DbService conn;
        public CuentasController(DbService connection, JWTService jwtService)
        {
            this.conn = connection;
            this._jwtService = jwtService;
        }
        [HttpPost("generate-token")]
        public IActionResult GenerateToken([FromBody] UserCredentials credentials)
        {
            // Aquí debes realizar la lógica de autenticación y validación de las credenciales del usuario.
            // Si las credenciales son válidas, puedes generar el token JWT.

            // Por ejemplo, supongamos que verificamos las credenciales y obtenemos el ID del usuario autenticado.

            string userId = new User(conn).Authenticate(credentials.Username/*, credentials.Password*/);
            if (userId != null)
            {
                // tokenJWT
                string token = _jwtService.GenerateToken(userId);

                // devolver
                return Ok(new { Token = token});
            }
            else
            {
                // user no existe
                return Unauthorized();
            }
        }
    }
}
