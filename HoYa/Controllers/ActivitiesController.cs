using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Web.Http.Description;
using System;

namespace HoYa.Controllers
{
    [Authorize]
    public class ActivitiesController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public IQueryable<Activity> GetActivities()
        {
            return db.Activities;
        }     

        public async Task<IHttpActionResult> GetActivity(Guid id)
        {
            Activity existedActivity = await db.Activities.FindAsync(id);
            if (existedActivity == null) return NotFound();
            return Ok(existedActivity);
        }

        public async Task<IHttpActionResult> PutActivity(Guid id, Activity mission)
        {
            Activity existedActivity = await db.Activities.FindAsync(id);
            mission.Id = existedActivity.Id;
            db.Entry(existedActivity).CurrentValues.SetValues(mission);
            await db.SaveChangesAsync();
            await db.Entry(existedActivity).GetDatabaseValuesAsync();
            return Ok(existedActivity);
        }

        [ResponseType(typeof(Activity))]
        public async Task<IHttpActionResult> PostActivity(Activity mission)
        {
            db.Activities.Add(mission);
            await db.SaveChangesAsync();
            await db.Entry(mission).GetDatabaseValuesAsync();
            return Ok(mission);
        }

        public async Task<IHttpActionResult> DeleteActivity(Guid id)
        {
            Activity existedActivity = await db.Activities.FindAsync(id);
            if (existedActivity == null) return NotFound();
            db.Activities.Remove(existedActivity);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}