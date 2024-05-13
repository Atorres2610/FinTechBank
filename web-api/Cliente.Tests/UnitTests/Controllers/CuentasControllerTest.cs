using Cliente.API.Controllers;
using Cliente.API.Helpers;
using Cliente.API.Models.Cuenta;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cliente.Tests.UnitTests.Controllers
{
    [TestClass]
    public class CuentasControllerTest : BasePruebas
    {
        [TestMethod]
        public async Task ObtenerUsuarioClaveIncorrecta()
        {
            //Preparación
            var nombreBd = Guid.NewGuid().ToString();
            var dbContext = ConstruirContext(nombreBd);
            dbContext.Usuarios.Add(new Infrastructure.Entities.Usuario { Clave = "claveprueba", Email = "a@a.com" });
            await dbContext.SaveChangesAsync();

            //Prueba
            var controller = new CuentasController(dbContext, new ConfigurationManager());
            var respuesta = await controller.Login(new UsuarioLogin("a@a.com", "otraClave"), default);

            //Verificación
            var resultado = respuesta.Result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status404NotFound, resultado?.StatusCode);
        }

        [TestMethod]
        public async Task ObtenerUsuarioCorrecto()
        {
            //Preparación
            var nombreBd = Guid.NewGuid().ToString();
            var dbContext = ConstruirContext(nombreBd);
            dbContext.Usuarios.Add(new Infrastructure.Entities.Usuario { Clave = "claveprueba".EncryptSha256(), Email = "a@a.com" });
            await dbContext.SaveChangesAsync();

            var dbContext2 = ConstruirContext(nombreBd);

            //Prueba
            var controller = new CuentasController(dbContext2, ObtenerConfiguracion());
            var respuesta = await controller.Login(new UsuarioLogin("a@a.com", "claveprueba"), default);

            //Verificación
            var resultado = respuesta.Result as ObjectResult;
            var usuarioToken = resultado?.Value as UsuarioToken;
            Assert.IsNotNull(usuarioToken?.Token);
        }

        [TestMethod]
        public async Task CrearUsuario()
        {
            //Preparación
            var nombreBd = Guid.NewGuid().ToString();
            var dbContext = ConstruirContext(nombreBd);
            var nuevoUsuario = new UsuarioInfo("a@a.com", "claveprueba");

            //Prueba
            var controller = new CuentasController(dbContext, ObtenerConfiguracion());
            var respuesta = await controller.CrearUsuario(nuevoUsuario, default);

            //Verificación
            //Verificación
            var resultado = respuesta.Result as ObjectResult;
            var usuarioToken = resultado?.Value as UsuarioToken;
            Assert.IsNotNull(usuarioToken?.Token);

            var dbContext2 = ConstruirContext(nombreBd);
            var cantidad = await dbContext2.Usuarios.CountAsync();
            Assert.AreEqual(1, cantidad);
        }

        private static IConfiguration ObtenerConfiguracion()
        {
            return new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }
    }
}