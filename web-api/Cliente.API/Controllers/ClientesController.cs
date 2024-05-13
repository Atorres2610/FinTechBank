using Cliente.API.Models.Clientes;
using Cliente.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cliente.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesErrorResponseType(typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly FinTechBankContext dbContext;

        public ClientesController(FinTechBankContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Infrastructure.Entities.Cliente>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Infrastructure.Entities.Cliente>>> Get(CancellationToken cancellationToken)
        {
            List<Infrastructure.Entities.Cliente> clientes = await dbContext.Clientes.AsNoTracking().ToListAsync(cancellationToken);
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Infrastructure.Entities.Cliente), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Infrastructure.Entities.Cliente>> Get(int id, CancellationToken cancellationToken)
        {
            var cliente = await dbContext.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            if (cliente is null)
            {
                return NotFound("El cliente no existe en la base de datos.");
            }

            return Ok(cliente);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> Post([FromBody] CrearCliente crearCliente, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                await dbContext.Clientes.AddAsync(crearCliente.GetToCliente(), cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);

                return Created();
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Put(int id, [FromBody] ActualizarCliente actualizarCliente, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                if (actualizarCliente.Id == id)
                {
                    dbContext.Update(actualizarCliente.GetToCliente());
                    await dbContext.SaveChangesAsync(cancellationToken);
                    return NoContent();
                }

                return BadRequest("El id del cliente no coincide.");
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var cliente = await dbContext.Clientes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (cliente == null)
            {
                return NotFound("No se encontró al cliente.");
            }

            dbContext.Remove(cliente);
            await dbContext.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}