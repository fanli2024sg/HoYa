using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using HoYa.Models;
using System.Web;

namespace HoYa.Controllers
{
    [Authorize]

    public class CategoriesController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public IQueryable<Category> GetCategories(
            string anyLike = "",
            Guid? statusId = null
        )
        {
            if (anyLike == null) anyLike = "";
            return db.Categories.Where(x =>
            ((x.Status.Value != "新" && statusId == null) || x.StatusId == statusId) &&
            x.Value.ToString().Contains(anyLike)
            );
        }

        [Route("api/Categories/Grid/{id}")]
        public async Task<IHttpActionResult> GetCategoryGrid(Guid id)
        {
            return Ok(await db.Categories.Where(x => x.Id == id).Select(category => new CategoryGrid
            {
                Category = category,
                ItemQuantity = db.Items.Where(item => (db.ItemCategories.Where(itemCategory => itemCategory.TargetId == category.Id && itemCategory.OwnerId == item.Id && itemCategory.EndDate == null).Count() > 0)).Count()
            }).FirstOrDefaultAsync());
        }

        [Route("api/Categories/Grid")]
        public IQueryable<CategoryGrid> GetCategoryGrids(
            string anyLike = ""
        )
        {
            return db.Categories.Where(x =>
            x.Value.ToString().Contains(anyLike)
            ).Select(category => new CategoryGrid
            {
                Category = category,
                ItemQuantity = db.ItemCategories.Where(itemCategory => itemCategory.TargetId == category.Id && itemCategory.EndDate == null).Count()
            }).OrderBy(x => x.Category.Value);
        }

        [Route("api/Categories/Select")]
        public IQueryable<CategorySelect> GetCategorySelects(
             string anyLike = ""
        )
        {
            if (anyLike == null) anyLike = "";
            else anyLike = HttpUtility.UrlDecode(anyLike);
            return db.Categories.Where(x =>
              x.Value.ToString().Contains(anyLike)
            ).Select(category =>new CategorySelect
            { 
                Id = category.Id, 
                Value = category.Value,
                ItemQuantity = db.ItemCategories.Where(itemCategory => itemCategory.TargetId == category.Id && itemCategory.EndDate == null).Count()
            }).OrderBy(x => x.Value);
        }

        [Route("api/Categories/Query")]
        public async Task<IHttpActionResult> GetCategoriesQuery(
            string anyLike = "",
            string sortBy = "",
            string orderBy = "",
            int? pageIndex = 1,
            int? pageSize = 20
        )
        {
            IQueryable<Category> categories = db.Categories.Where(x => (x.Value.ToString().Contains(anyLike) || anyLike == null) ||
                           (x.Code.ToString().Contains(anyLike) || anyLike == null));
            switch (sortBy)
            {
                default:
                    if (orderBy == "asc") return Ok(new Query<Category>
                    {
                        PaginatorLength = categories.Count(),
                        Data = categories.OrderBy(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<Category>()
                    });
                    else return Ok(new Query<Category>
                    {
                        PaginatorLength = categories.Count(),
                        Data = categories.OrderByDescending(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<Category>()
                    });
            }
        }

        public async Task<IHttpActionResult> GetCategory(Guid id)
        {
            Category existedCategory = await db.Categories.FindAsync(id);
            if (existedCategory == null) return NotFound();
            return Ok(existedCategory);
        }



        [Route("api/Categories/ByValue/{value}")]
        public async Task<IHttpActionResult> GetCategoryLists(Guid id)
        {
            Category existedCategory = await db.Categories.FindAsync(id);
            if (existedCategory == null) return NotFound();
            return Ok(existedCategory);
        } 

        public async Task<IHttpActionResult> PutCategory(Guid id, Category category)
        {
            Category existedCategory = await db.Categories.FindAsync(id);
            category.Id = existedCategory.Id;
            db.Entry(existedCategory).CurrentValues.SetValues(category);
            await db.SaveChangesAsync();
            await db.Entry(existedCategory).GetDatabaseValuesAsync();
            return Ok(existedCategory);
        }

        public async Task<IHttpActionResult> PostCategory(Category category)
        {
            Category existedCategory = await db.Categories.FirstOrDefaultAsync(x=>x.Value==category.Value);
            if(existedCategory!=null) return Ok(existedCategory);
            db.Categories.Add(category);
            await db.SaveChangesAsync();
            await db.Entry(category).GetDatabaseValuesAsync();
            return Ok(category);
        }

        public async Task<IHttpActionResult> DeleteCategory(Guid id)
        {
            Category existedCategory = await db.Categories.FindAsync(id);
            if (existedCategory == null) return NotFound();
            db.Categories.Remove(existedCategory);
            await db.SaveChangesAsync();
            return Ok();
        }

    }
}