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
    public class ContactsController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public IQueryable<Contact> GetContacts(
            Guid? ownerId = null)
        {
            return db.Contacts.Where(x => x.OwnerId == ownerId || ownerId == null);
        }    

        public async Task<IHttpActionResult> PostContact(Contact contact)
        {
            contact.Owner = await db.People.FindAsync(contact.OwnerId);
            db.Contacts.Add(contact);
            await db.SaveChangesAsync();
            return Ok(contact);
        }

        public async Task<IHttpActionResult> PutContact(Guid id, Contact contact)
        {
            Contact existedContact = await db.Contacts.FindAsync(id);
            db.Entry(existedContact).CurrentValues.SetValues(contact);
            await db.SaveChangesAsync();
            return Ok(existedContact);
        }

        public async Task<IHttpActionResult> DeleteContact(Guid id)
        {
            Contact existedContact = await db.Contacts.FindAsync(id);
            if(existedContact!=null) db.Contacts.Remove(existedContact); 
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}