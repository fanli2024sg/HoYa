using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using HoYa.Entities;
using System;
using System.Data.Entity;

namespace HoYa.Controllers
{

    public class ContactsController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        /// <summary>  
        /// 取得所有【畫廊】
        /// </summary>
        [ResponseType(typeof(Contact))]
        public IQueryable<Contact> GetContacts()
        {
            return db.Contacts;
        }

        /// <summary>  
        /// 透過【畫廊序號】取得【畫廊】
        /// </summary>
        /// <param name="id">畫廊序號</param>  
        [ResponseType(typeof(Contact))]
        public async Task<IHttpActionResult> GetContact(Guid id)
        {
            Contact contact = await db.Contacts.FindAsync(id);
            if (contact == null) return NotFound();
            return Ok(contact);
        }

        [Route("api/Contacts/ByProfile")]
        [ResponseType(typeof(IQueryable<Contact>))]
        public async Task<IHttpActionResult> GetContactsByProfileId(Guid id)
        {
            if (id == null) id = Guid.Parse("00000000-0000-0000-0000-000000000000");
            return Ok(db.Contacts.Where(x => x.OwnerId == id));
        }

        /// <summary>  
        /// 更新【畫廊】
        /// </summary>  
        /// <param name="id">畫廊序號</param>  
        /// <param name="contact">畫廊</param>
        public async Task<IHttpActionResult> PutContact(Guid id, Contact contact)
        {
            Contact existedContact = await db.Contacts.FindAsync(id);
            db.Entry(existedContact).CurrentValues.SetValues(contact);
            await db.SaveChangesAsync();
            await db.Entry(existedContact).GetDatabaseValuesAsync();
            return Ok(existedContact);
        }

        /// <summary>  
        /// 新增【畫廊】
        /// </summary>  
        /// <param name="contact"></param>
        public async Task<IHttpActionResult> PostContact(Contact contact)
        {
          
            contact.Id = Guid.NewGuid();
            db.Contacts.Add(contact);
            await db.SaveChangesAsync();
            await db.Entry(contact).GetDatabaseValuesAsync();
            return Ok(contact);
        }

        /// <summary>  
        /// 刪除【畫廊】
        /// </summary>  
        /// <param name="id">畫廊序號</param>
        public async Task<IHttpActionResult> DeleteContact(Guid id)
        {
            Contact existedContact = await db.Contacts.FindAsync(id);
            if (existedContact == null)return NotFound();
            db.Contacts.Remove(existedContact);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}