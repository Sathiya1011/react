using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
        [HttpGet]
        public IActionResult Get()
        {
            var result = from tbl_cart in _context.cart
                         join tbl_shoes in _context.shoes on tbl_cart.ShoeId equals tbl_shoes.ShoeId
                         select new
                         {
                             Name = tbl_cart.UserName,
                             ShoeName = tbl_shoes.Name,
                             Type = tbl_shoes.Type,
                             Color = tbl_shoes.Color,
                             Price = tbl_shoes.Price
                         };
            return Ok(result);
        }
        */

        [HttpGet("{username}/cartByName")]
        public IActionResult GetByName(string username)
        {
            var results = _context.cart.Where(n => n.UserName.Contains(username));

            var result = from tbl_cart in results
                         join tbl_ElectronicProducts in _context.ElectronicProducts on tbl_cart.ProductId equals tbl_ElectronicProducts.productId
                         select new
                         {
                             CartId = tbl_cart.CartId,
                             Name = tbl_cart.UserName,
                             ShoeName = tbl_ElectronicProducts.productname,
                             Type = tbl_ElectronicProducts.brand,
                             Color = tbl_ElectronicProducts.availablecount,
                             Quantity = tbl_cart.Quantity,
                             Price = tbl_ElectronicProducts.price
                         };

            return Ok(result);
        }

        [HttpGet("{username}/{shoeid}/cartByNameAndId")]
        public IActionResult GetByNameAndId(string username, int shoeid)
        {
            var results = _context.cart.Where(n => n.UserName.Contains(username) && n.ProductId == shoeid);
            return Ok(results);
        }

        [HttpGet("{id}/cartById")]
        public IActionResult GetCartById(int id)
        {
            var cart = _context.cart.Find(id);
            if (cart == null)
            {
                return NotFound();
            }

            return Ok(cart);
        }

        [HttpPost]
        public IActionResult Post(Cart cart)
        {
            _context.cart.Add(cart);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var cart = _context.cart.Find(id);
            if (cart == null)
            {
                return NotFound();
            }

            _context.cart.Remove(cart);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Cart cart)
        {
            if (id != cart.CartId)
            {
                return BadRequest();
            }

            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
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

        private bool CartExists(int id)
        {
            return _context.cart.Any(e => e.CartId == id);
        }

    }
}
