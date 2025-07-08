using API.Helpers;
using DTO.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetProducts")]
        public async Task<IActionResult> GetAll()        
        {
            try
            {
                var schema = AuthHelper.GetSchema(User);
                var list = await DAL.Org.Products.GetAll(schema);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao buscar produtos.", error = ex});
            }
        }

        [HttpGet("{id}", Name = "GetProductsById")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            try
            {
                var schema = AuthHelper.GetSchema(User);

                var obj = await DAL.Org.Products.GetById(schema, id);

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
                return BadRequest(new { message = "Erro ao buscar produtos.", error = ex});
            }
        }

        [HttpPost(Name = "SaveProduct")]
        public async Task<IActionResult> Save([FromBody] DTO.Org.Product product)
        {
            try
            {
                var schema = AuthHelper.GetSchema(User);

                var id = await DAL.Org.Products.Save(schema, product);
                
                return Ok(new { id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao buscar produtos.", error = ex });
            }
        }


        [HttpPost("{id}", Name = "DeleteProduct")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var schema = AuthHelper.GetSchema(User);

                var sucess = await DAL.Org.Products.DeleteById(schema, id);
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
                return BadRequest(new { message = "Erro ao buscar produtos.", error = ex });
            }
        }

    }
}
