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
    public class InventoryAttributesController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public async Task<IHttpActionResult> GetInventoryAttribute(Guid id)
        {
            InventoryAttribute inventoryAttribute = await db.InventoryAttributes.FindAsync(id);
            if (inventoryAttribute == null)
            {
                return NotFound();
            }

            return Ok(inventoryAttribute);
        }

        public async Task<IHttpActionResult> PostInventoryAttributeWith(InventoryAttribute inventoryAttribute)
        {         
            db.InventoryAttributes.Add(inventoryAttribute);
            await db.SaveChangesAsync();
            await db.Entry(inventoryAttribute).GetDatabaseValuesAsync();
            return Ok(inventoryAttribute);
        }

        public async Task<IHttpActionResult> PutAttributeWithInventory(Guid id, InventoryAttribute inventoryAttribute)
        {
            Entities.Attribute attribute = await db.Attributes.FirstOrDefaultAsync(x => x.Value == inventoryAttribute.Target.Value);
            if (attribute == null)
            {
                db.Attributes.Add(inventoryAttribute.Target);
                await db.SaveChangesAsync();
                attribute = await db.Attributes.FindAsync(inventoryAttribute.Target.Id);
            }
            inventoryAttribute.TargetId = attribute.Id;
            InventoryAttribute existedInventoryAttribute = await db.InventoryAttributes.FindAsync(id);
            db.Entry(existedInventoryAttribute).CurrentValues.SetValues(inventoryAttribute);
            await db.SaveChangesAsync();
            return Ok(existedInventoryAttribute);
        }


        public async Task<IHttpActionResult> GetInventoryAttributes(
            string anyLike = "",
            Guid? ownerId = null,
            int? take = 200
        )
        {
            if (anyLike == null) anyLike = "";
            else anyLike = HttpUtility.UrlDecode(anyLike);
            IQueryable<InventoryAttribute> inventoryAttributes = db.InventoryAttributes;
            if (ownerId != null) inventoryAttributes = inventoryAttributes.Where(inventoryAttribute => inventoryAttribute.OwnerId == ownerId);
            return Ok(inventoryAttributes.Take(take.GetValueOrDefault()).Select(x=>new { id=x.Id,ownerId=x.OwnerId,targetId=x.TargetId, startDate=x.StartDate})); 
        }

        public async Task<IHttpActionResult> DeleteInventoryAttribute(Guid id)
        {
            InventoryAttribute inventoryAttribute = await db.InventoryAttributes.FindAsync(id);
            if (inventoryAttribute == null) return NotFound();
            db.InventoryAttributes.Remove(inventoryAttribute);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}