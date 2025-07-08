using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Organization) ||
                string.IsNullOrWhiteSpace(request.User) ||
                string.IsNullOrWhiteSpace(request.Password))
                    return BadRequest("Campos obrigatórios.");

                // 1. Buscar schema da organização
                var schema = await DAL.Public.Organizations.GetByName(request.Organization);
                if (schema.Id == 0)
                    return BadRequest("Organização inválida.");

                // 2. Validar usuário e senha no schema correto
                var usuario = await DAL.Public.Users.GetByName(request.User, request.Password);
                if (usuario.Id == 0)
                    return BadRequest("Usuário ou senha inválidos.");

                // 3. Criar token JWT
                var key = Encoding.UTF8.GetBytes(JwtSettings.Key);

                var claims = new[]
                {
                new Claim("schema", schema.Schema_name),
                new Claim("codigo_usuario", usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Name),
            };

                var token = new JwtSecurityToken(
                    issuer: JwtSettings.Issuer,
                    audience: JwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(JwtSettings.ExpireMinutes),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                );

                //retorna toke e o tempo de expiração em segundos
                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expires_in = Convert.ToInt32(JwtSettings.ExpireMinutes * 60) });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, exception = ex.ToString() });
            }

        }

        public class LoginRequest
        {
            public string Organization { get; set; } = "";
            public string User { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty; // deve vir já como MD5
        }
    }

}
