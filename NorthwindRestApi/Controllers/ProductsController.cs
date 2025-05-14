using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NorthwindRestApi.Models;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly NorthwindOriginalContext _context;

        public ProductsController(NorthwindOriginalContext context)
        {
            _context = context;
        }

        // Hae kaikki tuotteet
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                return Ok(await _context.Products.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe: " + ex.Message);
            }
        }

        // Hae tuote pääavaimella (id)
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound($"Tuotetta id:llä {id} ei löydy");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe: " + ex.Message);
            }
        }

        // Haku muulla kuin pääavaimella (esim. categoryId)
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(int categoryId)
        {
            try
            {
                var products = await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe: " + ex.Message);
            }
        }

        // Lisää uusi tuote
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromBody] Product product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe: " + ex.Message);
            }
        }

        // Päivitä tuote
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromBody] Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest("Tuote-ID ei täsmää.");
            }
            try
            {
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Tuote päivitetty onnistuneesti.");
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe: " + ex.Message);
            }
        }

        // Poista tuote
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound($"Tuotetta id:llä {id} ei löydy");
                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return Ok($"Tuote {product.ProductName} poistettu onnistuneesti.");
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe: " + ex.Message);
            }
        }
    }
}
