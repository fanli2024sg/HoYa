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

    public class WorkFlowsController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        [Route("api/WorkFlows/Query")]
        public async Task<IHttpActionResult> GetWorkFlowsQuery(
            string anyLike = "",
            string sortBy = "",
            string orderBy = "",
            int? pageIndex = 1,
            int? pageSize = 20
        )
        {
            IQueryable<WorkFlow> workFlows = db.WorkFlows.Where(x => (x.Value.ToString().Contains(anyLike) || anyLike == null) ||
                           (x.Code.ToString().Contains(anyLike) || anyLike == null));
            switch (sortBy)
            {
                default:
                    if (orderBy == "asc") return Ok(new Query<WorkFlow>
                    {
                        PaginatorLength = workFlows.Count(),
                        Data = workFlows.OrderBy(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<WorkFlow>()
                    });
                    else return Ok(new Query<WorkFlow>
                    {
                        PaginatorLength = workFlows.Count(),
                        Data = workFlows.OrderByDescending(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<WorkFlow>()
                    });
            }
        }


        public IQueryable<WorkFlow> GetWorkFlows(string anyLike = "")
        {
            return db.WorkFlows.Where(x => (x.Value.ToString().Contains(anyLike) || anyLike == null) ||
                           (x.Code.ToString().Contains(anyLike) || anyLike == null));
        }


        public async Task<IHttpActionResult> GetWorkFlow(Guid id)
        {
            WorkFlow existedWorkFlow = await db.WorkFlows.FindAsync(id);
            if (existedWorkFlow == null) return NotFound();
            return Ok(existedWorkFlow);
        }
         
        public async Task<IHttpActionResult> PutWorkFlow(Guid id, WorkFlow workFlow)
        {
            WorkFlow existedWorkFlow = await db.WorkFlows.FindAsync(id);
            workFlow.Id = existedWorkFlow.Id;
            db.Entry(existedWorkFlow).CurrentValues.SetValues(workFlow);
            await db.SaveChangesAsync();
            await db.Entry(existedWorkFlow).GetDatabaseValuesAsync();
            return Ok(existedWorkFlow);
        }
 
        public async Task<IHttpActionResult> PostWorkFlow(WorkFlow workFlow)
        {
            db.WorkFlows.Add(workFlow);
            await db.SaveChangesAsync();
            await db.Entry(workFlow).GetDatabaseValuesAsync();
            return Ok(workFlow);
        }

        public async Task<IHttpActionResult> DeleteWorkFlow(Guid id)
        {
            WorkFlow existedWorkFlow = await db.WorkFlows.FindAsync(id);
            if (existedWorkFlow == null) return NotFound();
            db.WorkFlows.Remove(existedWorkFlow);
            await db.SaveChangesAsync();
            return Ok();
        }

    }
}