using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Models;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPut("increment-stock/{id}/{quantity}")]
        public async Task<IActionResult> IncrementQuantity( int Id, int Quantity)
        {
            var product = await _context.Products.FindAsync(request.Id);
            if (product == null)
            {
                return NotFound();
            }

            product.Quantity += request.Quantity;
            await _context.SaveChangesAsync();
            
            return Ok(product);
        }

        [HttpPut("decrement-stock/{id}/{quantity}")]")]
        public async Task<IActionResult> DecrementQuantity(int Id, int Quantity)
        {
            var product = await _context.Products.FindAsync(request.Id);
            if (product == null)
            {
                return NotFound();
            }

            if (product.Quantity < request.Quantity)
            {
                return BadRequest("Not enough quantity available.");
            }

            product.Quantity -= request.Quantity;
            await _context.SaveChangesAsync();
            
            return Ok(product);
        }
    }

    public class UpdateQuantityRequest
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
