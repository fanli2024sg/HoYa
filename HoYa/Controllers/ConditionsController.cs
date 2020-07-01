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
    public class ConditionsController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public async Task<IHttpActionResult> GetCondition(Guid id)
        {
            Condition existedCondition = await db.Conditions.FindAsync(id);
            if (existedCondition == null) return NotFound();
            return Ok(existedCondition);
        }

        public async Task<IHttpActionResult> PutCondition(Guid id, Condition condition)
        {
            Condition existedCondition = await db.Conditions.FindAsync(id);

            db.Entry(existedCondition).CurrentValues.SetValues(condition);
            await db.Entry(existedCondition).GetDatabaseValuesAsync();
            return Ok(existedCondition);
        }




        public IQueryable<Condition> GetConditions(
           string anyLike = ""
        )
        {
            return db.Conditions.Where(x => (x.Value.ToString().Contains(anyLike) || anyLike == null));
        }



        [Route("api/Conditions/Query")]
        public async Task<IHttpActionResult> GetConditionsQuery(
         string anyLike = "",
         string sortBy = "",
         string orderBy = "",
         int? pageIndex = 1,
         int? pageSize = 20
     )
        {
            IQueryable<Condition> conditions = db.Conditions.Where(x => (x.Value.ToString().Contains(anyLike) || anyLike == null));
            switch (sortBy)
            {
                default:
                    if (orderBy == "asc") return Ok(new Query<Condition>
                    {
                        PaginatorLength = conditions.Count(),
                        Data = conditions.OrderBy(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<Condition>()
                    });
                    else return Ok(new Query<Condition>
                    {
                        PaginatorLength = conditions.Count(),
                        Data = conditions.OrderByDescending(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<Condition>()
                    });
            }
        }

        public async Task<IHttpActionResult> PostCondition(Condition condition)
        {
            db.Conditions.Add(condition);
            await db.SaveChangesAsync();
            await db.Entry(condition).GetDatabaseValuesAsync();
            return Ok(condition);
        }

        public async Task<IHttpActionResult> DeleteCondition(Guid id)
        {
            Condition existedCondition = await db.Conditions.FindAsync(id);
            if (existedCondition == null) return NotFound();
            db.Conditions.Remove(existedCondition);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}