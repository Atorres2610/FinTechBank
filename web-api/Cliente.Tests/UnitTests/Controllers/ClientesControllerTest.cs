using Cliente.API.Controllers;
using Cliente.API.Models.Clientes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cliente.Tests.UnitTests.Controllers
{
    [TestClass]
    public class ClientesControllerTest : BasePruebas
    {
        [TestMethod]
        public async Task ObtenerTodosLosClientes()
        {
            //Preparación
            var nombreBd = Guid.NewGuid().ToString();
            var dbContext = ConstruirContext(nombreBd);
            await CrearGenerosTiposCliente(dbContext);
            await CreacionCliente(dbContext);

            var dbContext2 = ConstruirContext(nombreBd);

            //Prueba
            var controller = new ClientesController(dbContext2);
            var respuesta = await controller.Get(default);

            //Verificación
            var resultado = respuesta.Result as ObjectResult;
            var clientes = resultado?.Value as List<Infrastructure.Entities.Cliente>;
            Assert.AreEqual(1, clientes?.Count);
        }

        [TestMethod]
        public async Task ObtenerClientePorIdNoExistente()
        {
            //Preparación
            var nombreBd = Guid.NewGuid().ToString();
            var dbContext = ConstruirContext(nombreBd);

            //Prueba
            var controller = new ClientesController(dbContext);
            var respuesta = await controller.Get(1, default);

            //Verificación
            var resultado = respuesta.Result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status404NotFound, resultado?.StatusCode);
        }

        [TestMethod]
        public async Task ObtenerClientePorIdExistente()
        {
            //Preparación
            var nombreBd = Guid.NewGuid().ToString();
            var dbContext = ConstruirContext(nombreBd);
            await CrearGenerosTiposCliente(dbContext);
            await CreacionCliente(dbContext);

            var dbContext2 = ConstruirContext(nombreBd);

            //Prueba
            var idCliente = 1;
            var controller = new ClientesController(dbContext2);
            var respuesta = await controller.Get(idCliente, default);

            //Verificación
            var resultado = respuesta.Result as ObjectResult;
            var cliente = resultado?.Value as Infrastructure.Entities.Cliente;
            Assert.AreEqual(idCliente, cliente?.Id);
        }

        [TestMethod]
        public async Task CrearCliente()
        {
            //Preparación
            var nombreBd = Guid.NewGuid().ToString();
            var dbContext = ConstruirContext(nombreBd);
            await CrearGenerosTiposCliente(dbContext);

            var nuevoCliente = new CrearCliente("a", "b", "c", 10, DateTime.Now, "b", "12345678", "a@a.com", 1, "es", "as", "es", 1, "");

            //Prueba
            var controller = new ClientesController(dbContext);
            var respuesta = await controller.Post(nuevoCliente, default);

            //Verificación
            var resultado = respuesta as CreatedResult;
            Assert.IsNotNull(resultado);

            var dbContext2 = ConstruirContext(nombreBd);
            var cantidad = await dbContext2.Clientes.CountAsync();
            Assert.AreEqual(1, cantidad);
        }

        [TestMethod]
        public async Task ActualizarCliente()
        {
            //Preparación
            var nombreBd = Guid.NewGuid().ToString();
            var dbContext = ConstruirContext(nombreBd);
            await CrearGenerosTiposCliente(dbContext);
            await CreacionCliente(dbContext);

            var dbContext2 = ConstruirContext(nombreBd);

            //Prueba
            var idCliente = 1;
            var controller = new ClientesController(dbContext2);
            var cliente = new ActualizarCliente(idCliente, "nuevo nombre", "b", "c", 10, DateTime.Now, "b", "12345678", "a@a.com", 1, "es", "as", "es", 1, "");
            var respuesta = await controller.Put(idCliente, cliente, default);

            //Verificación
            var resultado = respuesta as StatusCodeResult;
            Assert.AreEqual(StatusCodes.Status204NoContent, resultado?.StatusCode);

            var dbContext3 = ConstruirContext(nombreBd);
            var existe = await dbContext3.Clientes.AnyAsync(x => x.Nombre.Equals("nuevo nombre"));
            Assert.IsTrue(existe);
        }

        [TestMethod]
        public async Task IntentaBorrarClienteNoExistente()
        {
            //Preparación
            var nombreBd = Guid.NewGuid().ToString();
            var dbContext = ConstruirContext(nombreBd);

            //Prueba
            var idCliente = 1;
            var controller = new ClientesController(dbContext);
            var respuesta = await controller.Delete(idCliente, default);

            //Verificación
            var resultado = respuesta as ObjectResult;
            Assert.AreEqual(StatusCodes.Status404NotFound, resultado?.StatusCode);
        }

        [TestMethod]
        public async Task BorrarCliente()
        {
            //Preparación
            var nombreBd = Guid.NewGuid().ToString();
            var dbContext = ConstruirContext(nombreBd);
            await CreacionCliente(dbContext);

            var dbContext2 = ConstruirContext(nombreBd);

            //Prueba
            var idCliente = 1;
            var controller = new ClientesController(dbContext2);
            var respuesta = await controller.Delete(idCliente, default);

            //Verificación
            var resultado = respuesta as StatusCodeResult;
            Assert.AreEqual(StatusCodes.Status204NoContent, resultado?.StatusCode);

            var dbContext3 = ConstruirContext(nombreBd);
            var existe = await dbContext3.Clientes.AnyAsync();
            Assert.IsFalse(existe);
        }

        private static async Task CrearGenerosTiposCliente(Infrastructure.Data.FinTechBankContext dbContext)
        {
            dbContext.Generos.Add(new Infrastructure.Entities.Genero { Nombre = "Genero 1" });
            dbContext.Generos.Add(new Infrastructure.Entities.Genero { Nombre = "Genero 2" });

            dbContext.TiposCliente.Add(new Infrastructure.Entities.TipoCliente { Nombre = "Tipo 1" });
            dbContext.TiposCliente.Add(new Infrastructure.Entities.TipoCliente { Nombre = "Tipo 2" });

            await dbContext.SaveChangesAsync();
        }

        private static async Task CreacionCliente(Infrastructure.Data.FinTechBankContext dbContext)
        {
            dbContext.Clientes.Add(new Infrastructure.Entities.Cliente
            {
                Apellido = "da",
                Correo = "a@a.com",
                Direccion = "a",
                EstadoCivil = "b",
                Nacionalidad = "c",
                Nombre = "d",
                NumeroCuenta = "e",
                NumeroIdentificacion = "f",
                ProfesionOcupacion = "g",
                Telefono = "h",
                FechaNacimiento = DateTime.Now,
                IdGenero = 1,
                IdTipoCliente = 2,
                Saldo = 3
            });

            await dbContext.SaveChangesAsync();
        }
    }
}