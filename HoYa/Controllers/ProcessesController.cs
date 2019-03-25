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

namespace HoYa.Controllers
{
    //[Authorize]

    public class ProcessesController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public IQueryable<Process> GetProcesses()
        {
            return db.Processes;
        }


        [ResponseType(typeof(Process))]
        public async Task<IHttpActionResult> GetProcess(Guid id)
        {
            Process existedProcess = await db.Processes.FindAsync(id);
            if (existedProcess == null) return NotFound();
            return Ok(existedProcess);
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