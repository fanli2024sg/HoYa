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

namespace HoYa.Controllers
{
    //[Authorize]

    public class StepsController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public async Task<IHttpActionResult> GetSteps(
            string anyLike = "",
            string sortBy = "",
            string orderBy = "",
            int? pageIndex = 1,
            int? pageSize = 20
        )
        {
            IQueryable<Step> steps = db.Steps.Where(x => (x.Owner.Value.Contains(anyLike) || anyLike == null) ||
                           (x.Owner.Code.Contains(anyLike) || anyLike == null) ||
                           (x.Value.Contains(anyLike) || anyLike == ""));
            switch (sortBy)
            {
                default:
                    if (orderBy == "asc") return Ok(new Query<Step>
                    {
                        PaginatorLength = steps.Count(),
                        Data = steps.OrderBy(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<Step>()
                    });
                    else return Ok(new Query<Step>
                    {
                        PaginatorLength = steps.Count(),
                        Data = steps.OrderByDescending(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<Step>()
                    });
            }
        }


        public async Task<IHttpActionResult> GetStep(Guid id)
        {
            Step existedStep = await db.Steps.FindAsync(id);
            if (existedStep == null) return NotFound();
            return Ok(existedStep);
        }
         
        public async Task<IHttpActionResult> PutStep(Guid id, Step step)
        {
            Step existedStep = await db.Steps.FindAsync(id);
            step.Id = existedStep.Id;
            db.Entry(existedStep).CurrentValues.SetValues(step);
            await db.SaveChangesAsync();
            await db.Entry(existedStep).GetDatabaseValuesAsync();
            return Ok(existedStep);
        }
 
        public async Task<IHttpActionResult> PostStep(Step step)
        {
            db.Steps.Add(step);
            await db.SaveChangesAsync();
            await db.Entry(step).GetDatabaseValuesAsync();
            return Ok(step);
        }

        public async Task<IHttpActionResult> DeleteStep(Guid id)
        {
            Step existedStep = await db.Steps.FindAsync(id);
            if (existedStep == null) return NotFound();
            db.Steps.Remove(existedStep);
            await db.SaveChangesAsync();
            return Ok();
        }

    }
}