using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System;
using HoYa.Models;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Web;

namespace HoYa.Controllers
{
    [Authorize]
    public class ItemAttributesController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public async Task<IHttpActionResult> GetItemAttribute(Guid id)
        {
            ItemAttribute itemAttribute = await db.ItemAttributes.FindAsync(id);
            if (itemAttribute == null)
            {
                return NotFound();
            }

            return Ok(itemAttribute);
        }

        public async Task<IHttpActionResult> PostItemAttribute(ItemAttribute itemAttribute)
        {
            Entities.Attribute attribute = db.Attributes.FirstOrDefault(x => x.Value == itemAttribute.Target.Value && x.Code == itemAttribute.Target.Code);

            if (attribute != null)
            {
                itemAttribute.Target = null;
                itemAttribute.TargetId = attribute.Id;
            }
            db.ItemAttributes.Add(itemAttribute);
            await db.SaveChangesAsync();
            await db.Entry(itemAttribute).GetDatabaseValuesAsync();
            return Ok(itemAttribute);
        }

        public async Task<IHttpActionResult> PutItemAttribute(Guid id, ItemAttribute itemAttribute)
        {
            Entities.Attribute attribute = await db.Attributes.FirstOrDefaultAsync(x => x.Value == itemAttribute.Target.Value && x.Code == itemAttribute.Target.Code);
        
            if (attribute == null)
            {
                itemAttribute.Target.Id = Guid.NewGuid();
                itemAttribute.TargetId = itemAttribute.Target.Id;
                db.Attributes.Add(itemAttribute.Target);
                await db.SaveChangesAsync();
                attribute = await db.Attributes.FindAsync(itemAttribute.TargetId);
            }
            else
            {
                db.Entry(attribute).CurrentValues.SetValues(itemAttribute.Target);
                await db.SaveChangesAsync();
            }
            ItemAttribute existedItemAttribute = await db.ItemAttributes.FindAsync(id);
            db.Entry(existedItemAttribute).CurrentValues.SetValues(itemAttribute);
            await db.SaveChangesAsync();
            return Ok(existedItemAttribute);
        }


        public IQueryable<ItemAttribute> GetItemAttribute(
            string anyLike = "",
            Guid? ownerId = null,
            int? take = 50
        )
        {
            if (anyLike == null) anyLike = "";
            else anyLike = HttpUtility.UrlDecode(anyLike);
            IQueryable<ItemAttribute> itemAttributes = db.ItemAttributes;
            if (ownerId != null) itemAttributes = itemAttributes.Where(itemAttribute => itemAttribute.OwnerId == ownerId);
            if (anyLike != "") itemAttributes = itemAttributes.Where(x =>
                x.Target.Value.ToString().Contains(anyLike) ||
                x.Target.ValueNumber.ToString().Contains(anyLike) ||
                x.Target.ValueType.Contains(anyLike)
            );
            return itemAttributes.Take(take.GetValueOrDefault());
        }

        public async Task<IHttpActionResult> DeleteItemAttribute(Guid id)
        {
            ItemAttribute itemAttribute = await db.ItemAttributes.FindAsync(id);
            if (itemAttribute == null) return NotFound();
            db.ItemAttributes.Remove(itemAttribute);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}