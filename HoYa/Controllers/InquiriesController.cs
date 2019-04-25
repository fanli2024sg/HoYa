using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Web.Http.Description;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using HoYa.Models;
using HoYa.Repository;
using System.Security.Claims;

namespace HoYa.Controllers
{
    [Authorize]
    public class InquiriesController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public IQueryable<Inquiry> GetInquiries(
            Guid? ownerId = null)
        {
            return db.Inquiries.Where(x => x.OwnerId == ownerId || ownerId == null);
        }    

        public async Task<IHttpActionResult> PostInquiry(Inquiry inquiry)
        {
            inquiry.Recipe = await db.Recipes.FindAsync(inquiry.RecipeId);
            db.Inquiries.Add(inquiry);
            await db.SaveChangesAsync();
            return Ok(inquiry);
        }

        public async Task<IHttpActionResult> PutInquiry(Guid id, Inquiry inquiry)
        {
            Inquiry existedInquiry = await db.Inquiries.FindAsync(id);
            db.Entry(existedInquiry).CurrentValues.SetValues(inquiry);
            await db.SaveChangesAsync();
            return Ok(existedInquiry);
        }

        public async Task<IHttpActionResult> DeleteInquiry(Guid id)
        {
            Inquiry existedInquiry = await db.Inquiries.FindAsync(id);
            if(existedInquiry!=null) db.Inquiries.Remove(existedInquiry); 
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}