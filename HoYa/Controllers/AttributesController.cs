using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Web.Http.Description;
using System;
using System.Web;
using HoYa.Models;

namespace HoYa.Controllers
{
    //[Authorize]
    public class AttributesController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public async Task<IHttpActionResult> GetAttribute(Guid id)
        {
            Entities.Attribute existedAttribute = await db.Attributes.FindAsync(id);
            if (existedAttribute == null) return NotFound();
            return Ok(existedAttribute);
        }

        public async Task<IHttpActionResult> PutAttribute(Guid id, Entities.Attribute attribute)
        {
            Entities.Attribute existedAttribute = await db.Attributes.FindAsync(id);

            db.Entry(existedAttribute).CurrentValues.SetValues(attribute);
            await db.Entry(existedAttribute).GetDatabaseValuesAsync();
            return Ok(existedAttribute);
        }

        public async Task<IHttpActionResult> GetAttributes(
            string anyLike = "",
            Guid? itemId = null,
              string itemIds = "",
            Guid? inventoryId = null
        )
        {
            if (anyLike == null) anyLike = "";
            else anyLike = HttpUtility.UrlDecode(anyLike);
            IQueryable<Entities.Attribute> attributes = db.Attributes;


            if (itemIds != "")
            {
                IQueryable<string> _itemIds = itemIds.Split(',').AsQueryable();
                var items = db.Items.Where(x => _itemIds.Contains(x.Id.ToString())).Distinct();
                attributes = db.ItemAttributes.Where(x => _itemIds.Contains(x.OwnerId.ToString())).Select(x => x.Target);


                IQueryable<Guid?> _categoryIds = db.ItemCategories.Where(x => _itemIds.Contains(x.OwnerId.ToString())).Select(x => x.TargetId).Distinct();
                attributes = attributes.Union(db.CategoryAttributes.Where(y => _categoryIds.Contains(y.OwnerId)).Select(x => x.Target));
            }
            else
            {
                if (itemId != null)
                {
                    attributes = db.ItemAttributes.Where(x => x.OwnerId == itemId).Select(x => x.Target);
                    attributes = attributes.Union(db.CategoryAttributes.Where(y => db.ItemCategories.Any(x => x.OwnerId == itemId && x.TargetId == y.OwnerId)).Select(x => x.Target));
                }
            }

            if (inventoryId != null)
            {
                attributes = db.ItemAttributes.Where(y => db.Inventories.Any(x => x.Position.TargetId == inventoryId && x.ItemId == y.OwnerId)).Select(x => x.Target);
            }

            if (anyLike != "") attributes = attributes.Where(x => x.Value.Contains(anyLike));
            return Ok(attributes.OrderBy(x => x.Value).Select(x => new
            {
                id = x.Id,
                value = x.Value,
                code = x.Code,
                categoryIds = x.CategoryIds,
                itemIds = x.ItemIds,
                valueType = x.ValueType
            }));
        }

        [Route("api/Attributes/Checkbox")]
        public async Task<IHttpActionResult> GetAttributesCheckbox(
            string anyLike = "",
            Guid? itemId = null,
            Guid? categoryId = null,
            Guid? inventoryId = null
        )
        {
            if (anyLike == null) anyLike = "";
            else anyLike = HttpUtility.UrlDecode(anyLike);
            IQueryable<Entities.Attribute> attributes = db.Attributes;
            if (itemId != null)
            {
                attributes = db.ItemAttributes.Where(x => x.OwnerId == itemId).Select(x => x.Target);
                attributes = attributes.Union(db.CategoryAttributes.Where(y => db.ItemCategories.Any(x => x.OwnerId == itemId && x.TargetId == y.OwnerId)).Select(x => x.Target));
            }

            if (inventoryId != null)
            {
                attributes = db.ItemAttributes.Where(y => db.Inventories.Any(x => x.Position.TargetId == inventoryId && x.ItemId == y.OwnerId)).Select(x => x.Target);
            }

            if (anyLike != "") attributes = attributes.Where(x => x.Value.Contains(anyLike));
            return Ok(attributes.Select(x => new { id = x.Id, value = x.Value, code = x.Code }));
        }

        [Route("api/Attributes/ByInventory/{inventoryId}")]
        public async Task<IHttpActionResult> GetAttributesByInventory(
           Guid? inventoryId = null
       )
        {
            Inventory inventory = await db.Inventories.FindAsync(inventoryId);
            var inventoryAttributes = db.InventoryAttributes.Where(x => x.OwnerId == inventoryId).Select(x => new AttributeWithValue
            {
                Id = x.Id,
                Value = x.Value,
                TargetId = x.TargetId,
                TargetValue = x.Target.Value,
                Level = x.Target.Level,
                ValueType = x.Target.ValueType,
                ItemIds=   x.Target.ItemIds,
                InventoryIds = x.Target.InventoryIds,
                CategoryIds = x.Target.CategoryIds
            });

            var attributes = inventoryAttributes.ToList();




            var itemAttributes = db.ItemAttributes.Where(x => x.OwnerId == inventory.ItemId).Select(x => x.Target);
            var categoryAttributes = db.CategoryAttributes.Where(y => db.ItemCategories.Any(x => x.OwnerId == inventory.ItemId && x.TargetId == y.OwnerId)).Select(x => x.Target);

            foreach (var itemAttribute in itemAttributes.Where(x => !inventoryAttributes.Any(y => y.TargetValue == x.Value)))
            {
                attributes.Add(new AttributeWithValue
                {
                    Id = Guid.NewGuid(),
                    Value = "",
                    TargetId = itemAttribute.Id,
                    TargetValue = itemAttribute.Value,
                    Level = itemAttribute.Level,
                    ValueType = itemAttribute.ValueType,
                    ItemIds = itemAttribute.ItemIds,
                    InventoryIds = itemAttribute.InventoryIds,
                    CategoryIds = itemAttribute.CategoryIds
                });
            }

            foreach (var categoryAttribute in categoryAttributes.Where(x => !inventoryAttributes.Any(y => y.TargetValue == x.Value) && !itemAttributes.Any(y => y.Value == x.Value)))
            {
                attributes.Add(new AttributeWithValue
                {
                    Id = Guid.NewGuid(),
                    Value = "",
                    TargetId = categoryAttribute.Id,
                    TargetValue = categoryAttribute.Value,
                    Level = categoryAttribute.Level,
                    ValueType = categoryAttribute.ValueType,
                    ItemIds = categoryAttribute.ItemIds,
                    InventoryIds = categoryAttribute.InventoryIds,
                    CategoryIds = categoryAttribute.CategoryIds
                });
            }


            return Ok(attributes);
        }

        [Route("api/Attributes/ByItem/{itemId}")]
        public async Task<IHttpActionResult> GetAttributesByItem(
            Guid? itemId = null
        )
        {
            var itemAttributes = db.ItemAttributes.Where(x => x.OwnerId == itemId).Select(x => x.Target).Select(x => new AttributeWithValue
            {
                Id = Guid.NewGuid(),
                Value = "",
                TargetId=x.Id,
                TargetValue = x.Value,
                Level = x.Level,
                ValueType = x.ValueType,
                ItemIds = x.ItemIds,
                InventoryIds = x.InventoryIds,
                CategoryIds = x.CategoryIds
            });
            var attributes = itemAttributes.ToList();
            var categoryAttributes = db.CategoryAttributes.Where(y => db.ItemCategories.Any(x => x.OwnerId == itemId && x.TargetId == y.OwnerId)).Select(x => x.Target);

            foreach (var categoryAttribute in categoryAttributes.Where(x => !itemAttributes.Any(y => y.TargetValue == x.Value)))
            {
                attributes.Add(new AttributeWithValue
                {
                    Id = Guid.NewGuid(),
                    Value = "",
                    TargetId = categoryAttribute.Id,
                    TargetValue = categoryAttribute.Value,
                    Level = categoryAttribute.Level,
                    ValueType = categoryAttribute.ValueType,
                    ItemIds = categoryAttribute.ItemIds,
                    InventoryIds = categoryAttribute.InventoryIds,
                    CategoryIds = categoryAttribute.CategoryIds
                });
            }
            return Ok(attributes);
        }


        public async Task<IHttpActionResult> PostAttribute(Entities.Attribute attribute)
        {
            db.Attributes.Add(attribute);
            await db.SaveChangesAsync();
            await db.Entry(attribute).GetDatabaseValuesAsync();
            return Ok(attribute);
        }

        public async Task<IHttpActionResult> DeleteAttribute(Guid id)
        {
            Entities.Attribute existedAttribute = await db.Attributes.FindAsync(id);
            if (existedAttribute == null) return NotFound();
            db.Attributes.Remove(existedAttribute);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}