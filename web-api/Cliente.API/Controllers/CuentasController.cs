using Cliente.API.Helpers;
using Cliente.API.Models.Cuenta;
using Cliente.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cliente.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesErrorResponseType(typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class CuentasController : ControllerBase
    {
        private readonly FinTechBankContext dbContext;
        private readonly IConfiguration configuration;

        public CuentasController(FinTechBankContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        [HttpPost]
        [ProducesResponseType(typeof(UsuarioToken), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<UsuarioToken>> CrearUsuario([FromBody] UsuarioInfo usuarioInfo, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var usuarioBd = await dbContext.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Equals(usuarioInfo.Email), cancellationToken);
                if (usuarioBd == null)
                {
                    await dbContext.Usuarios.AddAsync(usuarioInfo.GetToUsuario(), cancellationToken);
                    await dbContext.SaveChangesAsync(cancellationToken);

                    return Ok(ConstruirToken(usuarioInfo));
                }
                else
                {
                    return Conflict("El email ya se encuentra registrado en la base de datos, intente con uno diferente.");
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(UsuarioToken), StatusCodes.Status200OK)]
        public async Task<ActionResult<UsuarioToken>> Login([FromBody] UsuarioLogin usuarioLogin, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var usuarioBd = await dbContext.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Email.Equals(usuarioLogin.Email) && u.Clave.Equals(usuarioLogin.Clave.EncryptSha256()), cancellationToken);
                if (usuarioBd == null)
                {
                    return NotFound("Usuario o clave incorrecto.");
                }

                return Ok(ConstruirToken(usuarioLogin.GetUsuarioInfo()));
            }

            return BadRequest(ModelState);
        }

        private UsuarioToken ConstruirToken(UsuarioInfo usuarioInfo)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, usuarioInfo.Email),
                new(ClaimTypes.Email, usuarioInfo.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.Now.AddYears(1);

            var token = new JwtSecurityToken(
                    issuer: null,
                    audience: null,
                    claims: claims,
                    expires: expiration,
                    signingCredentials: credentials
                );

            return new UsuarioToken(new JwtSecurityTokenHandler().WriteToken(token), expiration);
        }
    }
}