using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactProject.Model;

namespace ReactProject.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ElectronicProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ElectronicProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ElectronicProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ElectronicProducts>>> GetElectronicProducts()
        {
            return await _context.ElectronicProducts.ToListAsync();
        }

        // GET: api/ElectronicProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ElectronicProducts>> GetElectronicProducts(int id)
        {
            var electronicProducts = await _context.ElectronicProducts.FindAsync(id);

            if (electronicProducts == null)
            {
                return NotFound();
            }

            return electronicProducts;
        }

        // PUT: api/ElectronicProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutElectronicProducts(int id, ElectronicProducts electronicProducts)
        {
            if (id != electronicProducts.productId)
            {
                return BadRequest();
            }

            _context.Entry(electronicProducts).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElectronicProductsExists(id))
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

        // POST: api/ElectronicProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ElectronicProducts>> PostElectronicProducts(ElectronicProducts electronicProducts)
        {
            _context.ElectronicProducts.Add(electronicProducts);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetElectronicProducts", new { id = electronicProducts.productId }, electronicProducts);
        }

        // DELETE: api/ElectronicProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteElectronicProducts(int id)
        {
            var electronicProducts = await _context.ElectronicProducts.FindAsync(id);
            if (electronicProducts == null)
            {
                return NotFound();
            }

            _context.ElectronicProducts.Remove(electronicProducts);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ElectronicProductsExists(int id)
        {
            return _context.ElectronicProducts.Any(e => e.productId == id);
        }
    }
}
