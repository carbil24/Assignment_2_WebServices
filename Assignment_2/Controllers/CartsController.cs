using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Assignment_2.Models;

namespace Assignment_2.Controllers
{
    public class CartsController : ApiController
    {
        private EcommerceDataBaseConnection db = new EcommerceDataBaseConnection();

        // GET: api/Carts
        public IQueryable<Cart> GetCarts()
        {
            return db.Carts;
        }

        // GET: api/Carts/5
        [ResponseType(typeof(Cart))]
        public async Task<IHttpActionResult> GetCart(int id)
        {
            Cart cart = await db.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            return Ok(cart);
        }

        // GET: api/Carts/5/CartItems
        [HttpGet()]
        [Route("api/Carts/{id}/CartItems")]
        [ResponseType(typeof(CartItem))]
        public async Task<IHttpActionResult> GetCartItems([FromUri] int id)
        {
            Cart cart = await db.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            return Ok(cart.CartItems);
        }

        // GET: api/Carts/5/MethodOfPayment
        [HttpGet()]
        [Route("api/Carts/{id}/MethodOfPayment")]
        [ResponseType(typeof(MethodOfPayment))]
        public async Task<IHttpActionResult> GetCartMethodOfPayment([FromUri] int id)
        {
            Cart cart = await db.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            return Ok(cart.MethodOfPayment);
        }


        // PUT: api/Carts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCart(int id, Cart cart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cart.CartId)
            {
                return BadRequest();
            }

            db.Entry(cart).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Carts
        [ResponseType(typeof(Cart))]
        public async Task<IHttpActionResult> PostCart(Cart cart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Carts.Add(cart);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = cart.CartId }, cart);
        }

        // DELETE: api/Carts/5
        [ResponseType(typeof(Cart))]
        public async Task<IHttpActionResult> DeleteCart(int id)
        {
            Cart cart = await db.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            if (db.CartItems.Where(u => u.CartId == id).Any() == true)
            {
                CartItem cartItem = db.CartItems.Where(u => u.CartId == id).FirstOrDefault();
                Product product = await db.Products.FindAsync(cartItem.ProductId);

                product.Stock += cartItem.Quantity;

                db.Entry(product).State = EntityState.Modified;

                db.CartItems.Remove(cartItem);
            }

            db.Carts.Remove(cart);
            await db.SaveChangesAsync();

            return Ok(cart);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CartExists(int id)
        {
            return db.Carts.Count(e => e.CartId == id) > 0;
        }
    }
}