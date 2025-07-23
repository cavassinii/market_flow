using API;
using DTO.Mercado_Livre;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MercadoLivreController : ControllerBase
    {
        private readonly ILogger<MercadoLivreController> _logger;

        public MercadoLivreController(ILogger<MercadoLivreController> logger)
        {
            _logger = logger;
        }
        [HttpGet("GetCategoryTree")]
        public async Task<IActionResult> GetAllCategoriesMl()
        {
            try
            {
                // Busca a árvore completa de categorias
                var categoryTree = await DAL.Mercado_Livre.Categories.GetFullCategoryTreeAsync();

                // Retorna 200 OK com o objeto JSON da árvore
                return Ok(categoryTree);
            }
            catch (Exception ex)
            {
                // Em caso de erro, retorna 500 com mensagem e detalhes do erro
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "Erro ao buscar as categorias.", Detail = ex.Message });
            }
        }


    }
}