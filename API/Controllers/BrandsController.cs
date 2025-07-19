using API.Helpers;
using DTO.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly ILogger<BrandsController> _logger;

        public BrandsController(ILogger<BrandsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetBrands")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var schema = AuthHelper.GetSchema(User);
                var list = await DAL.Org.Brands.GetAll(schema);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao buscar marcas.", error = ex });
            }
        }

        [HttpGet("{id}", Name = "GetBrandsById")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var schema = AuthHelper.GetSchema(User);

                var obj = await DAL.Org.Brands.GetById(schema, id);

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
                return BadRequest(new { message = "Erro ao buscar marcas.", error = ex });
            }
        }

        [HttpPost(Name = "SaveBrand")]
        public async Task<IActionResult> Save([FromBody] DTO.Org.Brand brand)
        {
            try
            {
                var schema = AuthHelper.GetSchema(User);

                var id = await DAL.Org.Brands.Save(schema, brand);

                return Ok(new { id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao salvar marca.", error = ex });
            }
        }


        [HttpDelete("{id}", Name = "DeleteBrand")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var schema = AuthHelper.GetSchema(User);

                var sucess = await DAL.Org.Brands.DeleteById(schema, id);
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
                return BadRequest(new { message = "Erro deletar marca.", error = ex });
            }
        }
    }       
}
