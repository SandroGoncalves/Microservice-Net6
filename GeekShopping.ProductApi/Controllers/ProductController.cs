using GeekShopping.ProductApi.Data.ValueObj;
using GeekShopping.ProductApi.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.ProductApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository ?? throw new ArgumentException(nameof(repository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductVO>>> FindAll()
        {
            var products = await _repository.FindAll();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductVO>> FindById(long id)
        {
            var product = await _repository.FindById(id);
            if (product.Id <= 0) return NotFound();

            return Ok(product);
        }
        
        [HttpPost]
        public async Task<ActionResult<ProductVO>> CreateProduct([FromBody] ProductVO product)
        {
            if (product == null) return BadRequest();
            var createProduct = await _repository.Create(product);

            return Ok(createProduct);
        } 
        
        [HttpPut]
        public async Task<ActionResult<ProductVO>> UpdateProduct([FromBody] ProductVO product)
        {
            if (product == null) return BadRequest();
            var UpdateProduct = await _repository.Update(product);

            return Ok(UpdateProduct);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(long id)
        {
            var DeleteProductStatus = await _repository.Delete(id);
            if(!DeleteProductStatus) return BadRequest();

            return Ok(DeleteProductStatus);
        }

    }
}
