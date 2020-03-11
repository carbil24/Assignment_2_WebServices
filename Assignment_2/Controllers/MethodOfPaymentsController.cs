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
    public class MethodOfPaymentsController : ApiController
    {
        private EcommerceDataBaseConnection db = new EcommerceDataBaseConnection();

        // GET: api/MethodOfPayments
        public IQueryable<MethodOfPayment> GetMethodOfPayments()
        {
            return db.MethodOfPayments;
        }

        // GET: api/MethodOfPayments/5
        [ResponseType(typeof(MethodOfPayment))]
        public async Task<IHttpActionResult> GetMethodOfPayment(int id)
        {
            MethodOfPayment methodOfPayment = await db.MethodOfPayments.FindAsync(id);
            if (methodOfPayment == null)
            {
                return NotFound();
            }

            return Ok(methodOfPayment);
        }

        // PUT: api/MethodOfPayments/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMethodOfPayment(int id, MethodOfPayment methodOfPayment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != methodOfPayment.MethodOfPaymentId)
            {
                return BadRequest();
            }

            db.Entry(methodOfPayment).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MethodOfPaymentExists(id))
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

        // POST: api/MethodOfPayments
        [ResponseType(typeof(MethodOfPayment))]
        public async Task<IHttpActionResult> PostMethodOfPayment(MethodOfPayment methodOfPayment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MethodOfPayments.Add(methodOfPayment);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = methodOfPayment.MethodOfPaymentId }, methodOfPayment);
        }

        // DELETE: api/MethodOfPayments/5
        [ResponseType(typeof(MethodOfPayment))]
        public async Task<IHttpActionResult> DeleteMethodOfPayment(int id)
        {
            MethodOfPayment methodOfPayment = await db.MethodOfPayments.FindAsync(id);
            if (methodOfPayment == null)
            {
                return NotFound();
            }

            db.MethodOfPayments.Remove(methodOfPayment);
            await db.SaveChangesAsync();

            return Ok(methodOfPayment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MethodOfPaymentExists(int id)
        {
            return db.MethodOfPayments.Count(e => e.MethodOfPaymentId == id) > 0;
        }
    }
}