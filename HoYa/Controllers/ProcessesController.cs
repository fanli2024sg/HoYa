using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Web;
using HoYa.Models;

namespace HoYa.Controllers
{
    [Authorize]

    public class ProcessesController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        [ResponseType(typeof(Process))]
        public async Task<IHttpActionResult> GetProcess(Guid id)
        {
            Process existedProcess = await db.Processes.FindAsync(id);
            if (existedProcess == null) return NotFound();
            return Ok(existedProcess);
        }

        public async Task<IHttpActionResult> GetProcesses(
            string anyLike = "",
            bool byInventory = false,
            string sortBy = "",
            string orderBy = "",
            int? pageIndex = 1,
            int? pageSize = 20
        )
        {

           var bb =  db.Processes.Where(x =>
            
             
             
             db.Activities.Where(activity => activity.ArchivedDate == null).Any(activity => activity.TargetId == x.CurrentId)




              





              ).ToList();

            IQueryable <Process> processes = db.Processes.Where(x =>
             (
             
             
          (   byInventory == db.Steps.Where(step =>
              db.StepGroups.Where(stepGroup =>
              db.Inventories.Where(group =>
              db.Relationships.Where(inventoryGroup =>
              inventoryGroup.OwnerId == db.Inventories.FirstOrDefault(inventory => inventory.UserId == HttpContext.Current.User.Identity.Name).Id)
              .Any(inventoryGroup => inventoryGroup.TargetId == group.Id))
              .Any(group => group.Id == stepGroup.TargetId))
              .Any(stepGroup => stepGroup.OwnerId == step.Id))
              .Any(step => step.Id == x.CurrentId)) 
              
              
              
              || db.Activities.Where(activity => activity.ArchivedDate == null).Any(activity => activity.TargetId ==x.CurrentId)




              )



              &&
              ((x.WorkFlow.Value.ToString().Contains(anyLike) || anyLike == null) ||
              (x.No.ToString().Contains(anyLike) || anyLike == null))




               
              );

            switch (sortBy)
            {
                default:
                    if (orderBy == "asc") return Ok(new Query<Process>
                    {
                        PaginatorLength = processes.Count(),
                        Data = processes.OrderBy(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<Process>()
                    });
                    else return Ok(new Query<Process>
                    {
                        PaginatorLength = processes.Count(),
                        Data = processes.OrderByDescending(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<Process>()
                    });
            }
        }

        [ResponseType(typeof(Process))]
        public async Task<IHttpActionResult> PutProcess(Guid id, Process process)
        {
            Process existedProcess = await db.Processes.FindAsync(id);
            process.Id = existedProcess.Id;
            db.Entry(existedProcess).CurrentValues.SetValues(process);
            await db.SaveChangesAsync();
            await db.Entry(existedProcess).GetDatabaseValuesAsync();
            return Ok(existedProcess);
        }

        [ResponseType(typeof(Process))]
        public async Task<IHttpActionResult> PostProcess(Process process)
        {
            process.Id = Guid.NewGuid();
            process.Parent = await db.Processes.FindAsync(process.ParentId);
            process.CreatedDate = DateTime.Now;
            db.Processes.Add(process);
            await db.SaveChangesAsync();
            await db.Entry(process).GetDatabaseValuesAsync();
            return Ok(process);
        }

        public async Task<IHttpActionResult> DeleteProcess(Guid id)
        {
            Process existedProcess = await db.Processes.FindAsync(id);
            if (existedProcess == null) return NotFound();
            db.Processes.Remove(existedProcess);
            await db.SaveChangesAsync();
            return Ok();
        }

    }
}