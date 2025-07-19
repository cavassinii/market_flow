using API.Helpers;
using DTO.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{ 
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ILogger<CategoriesController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetCategories")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var schema = AuthHelper.GetSchema(User);
                var list = await DAL.Org.Categories.GetAll(schema);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao buscar categorias.", error = ex });
            }
        }

        [HttpGet("{id}", Name = "GetCategoriesById")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var schema = AuthHelper.GetSchema(User);

                var obj = await DAL.Org.Categories.GetById(schema, id);

                if (obj.Id == 0)
                {
                    return Ok(new { id });
                }
                else
                {
                    return Ok(obj);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao buscar categorias.", error = ex });
            }
        }

        [HttpPost(Name = "SaveCategorie")]
        public async Task<IActionResult> Save([FromBody] DTO.Org.Categorie Categorie)
        {
            try
            {
                var schema = AuthHelper.GetSchema(User);

                var id = await DAL.Org.Categories.Save(schema, Categorie);

                return Ok(new { id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao salvar categoria.", error = ex });
            }
        }


        [HttpDelete("{id}", Name = "DeleteCategorie")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var schema = AuthHelper.GetSchema(User);

                var sucess = await DAL.Org.Categories.DeleteById(schema, id);
                if (!sucess)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(new { id });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao deletar categoria.", error = ex });
            }
        }
        
    }
}
