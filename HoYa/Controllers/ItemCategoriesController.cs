using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System;
using System.Web.Http.Description;
using HoYa.Models;
namespace HoYa.Controllers
{
    [Authorize]
    public class ItemCategoriesController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public async Task<IHttpActionResult> GetItemCategory(Guid id)
        {
            ItemCategory itemCategory = await db.ItemCategories.FindAsync(id);
            if (itemCategory == null)
            {
                return NotFound();
            }

            return Ok(itemCategory);
        }

        [Route("api/ItemCategories/Query")]
        public async Task<IHttpActionResult> GetItemCategoriesQuery(
            string anyLike = "",
            string sortBy = "",
            string orderBy = "",
            int? pageIndex = 1,
            int? pageSize = 20
        )
        {
            IQueryable<ItemCategory> itemCategories = db.ItemCategories.Where(x =>
            (x.Target.Value.ToString().Contains(anyLike) || anyLike == null) ||
            (x.Owner.Value.ToString().Contains(anyLike) || anyLike == null)
            );
            switch (sortBy)
            {
                default:
                    if (orderBy == "asc") return Ok(new Query<ItemCategory>
                    {
                        PaginatorLength = itemCategories.Count(),
                        Data = itemCategories.OrderBy(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<ItemCategory>()
                    });
                    else return Ok(new Query<ItemCategory>
                    {
                        PaginatorLength = itemCategories.Count(),
                        Data = itemCategories.OrderByDescending(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<ItemCategory>()
                    });
            }
        }

        public IQueryable<ItemCategory> GetItemCategories(
              Guid? ownerId = null
        )
        {
            return db.ItemCategories.Where(x =>
            x.OwnerId == ownerId || ownerId == null
            );
        }

        public async Task<IHttpActionResult> PutItemCategory(Guid id, ItemCategory itemCategory)
        {
            ItemCategory existedItemCategory = await db.ItemCategories.FindAsync(id);
            db.Entry(existedItemCategory).CurrentValues.SetValues(itemCategory);
            await db.SaveChangesAsync();
            return Ok(existedItemCategory);
        }

        public async Task<IHttpActionResult> PostItemCategory(ItemCategory itemCategory)
        {
            itemCategory.Target = await db.Categories.FindAsync(itemCategory.TargetId);
            db.ItemCategories.Add(itemCategory);
            await db.SaveChangesAsync();
            await db.Entry(itemCategory).GetDatabaseValuesAsync();
            return Ok(itemCategory);
        }
        public async Task<IHttpActionResult> DeleteItemCategory(Guid id)
        {
            ItemCategory itemCategory = await db.ItemCategories.FindAsync(id);
            if (itemCategory == null) return NotFound();
            db.ItemCategories.Remove(itemCategory);
            await db.SaveChangesAsync();
            return Ok(itemCategory);
        }
    }

}