using HoYa.Entities;
using HoYa.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Security.Claims;
using System.Web.Http;

#pragma warning disable 1591

namespace HoYa.Repository
{
    [Authorize]
    public class WorkFlowsRepository : IDisposable
    {
        private HoYaContext db;
        public WorkFlowsRepository(HoYaContext db)
        {
            this.db = db;
        }

        public async Task<Activity> CreateActivities(Process process)
        {

            foreach (Step step in db.Steps.Where(x => x.OwnerId == process.WorkFlowId).OrderBy(x => x.Sort))
            {
                if (step.Sort == (int)step.Sort)
                {
                    db.Activities.Add(new Activity
                    {
                       // ReEdit = process.ReEdit,
                        OwnerId = process.Id,
                        TargetId = step.Id
                    });
                }
            }
            await db.SaveChangesAsync();
            Activity activity = await db.Activities.FirstOrDefaultAsync(x => x.TargetId == process.WorkFlow.FirstId);
            return activity;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}