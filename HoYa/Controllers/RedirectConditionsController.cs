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
    [Authorize]
    public class RedirectConditionsController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public async Task<IHttpActionResult> GetRedirectCondition(Guid id)
        {
            RedirectCondition existedRedirectCondition = await db.RedirectConditions.FindAsync(id);
            if (existedRedirectCondition == null) return NotFound();
            return Ok(existedRedirectCondition);
        }

        public async Task<IHttpActionResult> PutRedirectCondition(Guid id, RedirectCondition redirectCondition)
        {
            RedirectCondition existedRedirectCondition = await db.RedirectConditions.FindAsync(id);

            db.Entry(existedRedirectCondition).CurrentValues.SetValues(redirectCondition);
            await db.Entry(existedRedirectCondition).GetDatabaseValuesAsync();
            return Ok(existedRedirectCondition);
        }




        public IQueryable<RedirectCondition> GetRedirectConditions(
           Guid? ownerId = null
        )
        {
            return db.RedirectConditions.Where(x => (x.OwnerId== ownerId || ownerId == null));
        }



        [Route("api/RedirectConditions/Query")]
        public async Task<IHttpActionResult> GetRedirectConditionsQuery(
         string anyLike = "",
         string sortBy = "",
         string orderBy = "",
         int? pageIndex = 1,
         int? pageSize = 20
     )
        {
            IQueryable<RedirectCondition> redirectConditions = db.RedirectConditions.Where(x => (x.Target.Value.ToString().Contains(anyLike) || anyLike == null));
            switch (sortBy)
            {
                default:
                    if (orderBy == "asc") return Ok(new Query<RedirectCondition>
                    {
                        PaginatorLength = redirectConditions.Count(),
                        Data = redirectConditions.OrderBy(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<RedirectCondition>()
                    });
                    else return Ok(new Query<RedirectCondition>
                    {
                        PaginatorLength = redirectConditions.Count(),
                        Data = redirectConditions.OrderByDescending(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<RedirectCondition>()
                    });
            }
        }

        public async Task<IHttpActionResult> PostRedirectCondition(RedirectCondition redirectCondition)
        {
            db.RedirectConditions.Add(redirectCondition);
            await db.SaveChangesAsync();
            await db.Entry(redirectCondition).GetDatabaseValuesAsync();
            return Ok(redirectCondition);
        }

        public async Task<IHttpActionResult> DeleteRedirectCondition(Guid id)
        {
            RedirectCondition existedRedirectCondition = await db.RedirectConditions.FindAsync(id);
            if (existedRedirectCondition == null) return NotFound();
            db.RedirectConditions.Remove(existedRedirectCondition);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}