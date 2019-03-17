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

        public IQueryable<Processing> GetProcesses()
        {
            return db.Processes;
        }
        [Route("api/Processes/No")]
        public async Task<IHttpActionResult> GetProcessNo(Guid? typeId=null)
        {
            Option type = await db.Options.FindAsync(typeId);
            JObject process = new JObject();
            string no = type.Code + DateTime.Now.ToString("yyyyMM");
            process["no"] = no + (db.Processes.Where(x => x.TypeId == typeId && x.No.Substring(0, 10) == no).Count()+1).ToString("0000");
            return Ok(process);
        }
        [Route("api/Processes/By")]
        [ResponseType(typeof(Processing))]
        public IQueryable<Processing> GetProcessesBy(
            Guid? typeId = null,
            string anyLike = "")
        {
            return db.Processes.Where(x => (x.TypeId == typeId || typeId == null)).OrderBy(x => x.No);
        }

        [ResponseType(typeof(Processing))]
        public async Task<IHttpActionResult> GetProcess(Guid id)
        {
            Processing existedProcess = await db.Processes.FindAsync(id);
            if (existedProcess == null) return NotFound();
            return Ok(existedProcess);
        }

        [ResponseType(typeof(Processing))]
        public async Task<IHttpActionResult> PutProcess(Guid id, Processing process)
        {
            Processing existedProcess = await db.Processes.FindAsync(id);
            process.Id = existedProcess.Id;
            process.UpdatedDate = DateTime.Now;
            db.Entry(existedProcess).CurrentValues.SetValues(process);
            await db.SaveChangesAsync();
            await db.Entry(existedProcess).GetDatabaseValuesAsync();
            return Ok(existedProcess);
        }

        [ResponseType(typeof(Processing))]
        public async Task<IHttpActionResult> PostProcess(Processing process)
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
            Processing existedProcess = await db.Processes.FindAsync(id);
            if (existedProcess == null) return NotFound();
            db.Processes.Remove(existedProcess);
            await db.SaveChangesAsync();
            return Ok();
        }

    }
}