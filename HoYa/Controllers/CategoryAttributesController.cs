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
    public class CategoryAttributesController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public async Task<IHttpActionResult> GetCategoryAttribute(Guid id)
        {
            CategoryAttribute categoryAttribute = await db.CategoryAttributes.FindAsync(id);
            if (categoryAttribute == null)
            {
                return NotFound();
            }

            return Ok(categoryAttribute);
        }

        public async Task<IHttpActionResult> PostCategoryAttribute(CategoryAttribute categoryAttribute)
        {
            Entities.Attribute attribute = db.Attributes.FirstOrDefault(x => x.Value == categoryAttribute.Target.Value && x.Code == categoryAttribute.Target.Code);

            if (attribute != null)
            {
                categoryAttribute.Target = null;
                categoryAttribute.TargetId = attribute.Id;
            }
            db.CategoryAttributes.Add(categoryAttribute);
            await db.SaveChangesAsync();
            await db.Entry(categoryAttribute).GetDatabaseValuesAsync();
            return Ok(categoryAttribute);
        }

        public async Task<IHttpActionResult> PutCategoryAttribute(Guid id, CategoryAttribute categoryAttribute)
        {
            Entities.Attribute attribute = await db.Attributes.FirstOrDefaultAsync(x => x.Value == categoryAttribute.Target.Value && x.Code == categoryAttribute.Target.Code);
        
            if (attribute == null)
            {
                categoryAttribute.Target.Id = Guid.NewGuid();
                categoryAttribute.TargetId = categoryAttribute.Target.Id;
                db.Attributes.Add(categoryAttribute.Target);
                await db.SaveChangesAsync();
                attribute = await db.Attributes.FindAsync(categoryAttribute.TargetId);
            }
            else
            {
                db.Entry(attribute).CurrentValues.SetValues(categoryAttribute.Target);
                await db.SaveChangesAsync();
            }
            CategoryAttribute existedCategoryAttribute = await db.CategoryAttributes.FindAsync(id);
            db.Entry(existedCategoryAttribute).CurrentValues.SetValues(categoryAttribute);
            await db.SaveChangesAsync();
            return Ok(existedCategoryAttribute);
        }


        public IQueryable<CategoryAttribute> GetCategoryAttribute(
            string anyLike = "",
            Guid? ownerId = null,
            int? take = 50
        )
        {
            if (anyLike == null) anyLike = "";
            else anyLike = HttpUtility.UrlDecode(anyLike);
            IQueryable<CategoryAttribute> categoryAttributes = db.CategoryAttributes;
            if (ownerId != null) categoryAttributes = categoryAttributes.Where(categoryAttribute => categoryAttribute.OwnerId == ownerId);
            if (anyLike != "") categoryAttributes = categoryAttributes.Where(x =>
                x.Target.Value.ToString().Contains(anyLike) ||
                x.Target.ValueNumber.ToString().Contains(anyLike) ||
                x.Target.ValueType.Contains(anyLike)
            );
            return categoryAttributes.Take(take.GetValueOrDefault());
        }

        public async Task<IHttpActionResult> DeleteCategoryAttribute(Guid id)
        {
            CategoryAttribute categoryAttribute = await db.CategoryAttributes.FindAsync(id);
            if (categoryAttribute == null) return NotFound();
            db.CategoryAttributes.Remove(categoryAttribute);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}