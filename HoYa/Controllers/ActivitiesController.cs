using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System;
using HoYa.Models;

namespace HoYa.Controllers
{

    [Authorize]
    public class ActivitiesController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public async Task<IHttpActionResult> GetActivity(Guid id)
        {
            Activity existedActivity = await db.Activities.FindAsync(id);
            if (existedActivity == null) return NotFound();
            return Ok(existedActivity);
        }

        public async Task<IHttpActionResult> PutActivity(Guid id, Activity activity)
        {
            Activity existedActivity = await db.Activities.FindAsync(id);
            db.Entry(existedActivity).CurrentValues.SetValues(activity);
            await db.SaveChangesAsync();
            await db.Entry(existedActivity).GetDatabaseValuesAsync();
            return Ok(existedActivity);
        }

        public async Task<IHttpActionResult> GetActivities(
            string anyLike = "",
            string stepComponent = "",
            Guid? ownerId = null,
            Guid? acceptableId = null,//空值表示不篩選 帶值表示這個人(acceptableId==inventoryGroup.OwnerId且participates.ParticipantId==acceptableId的數量必須為0表示還沒接到)可接的活動
            string sortBy = "",
            string orderBy = "",
            int? pageIndex = 1,
            int? pageSize = 20
        )
        {
            if (anyLike == "") anyLike = null;
            if (stepComponent == "") stepComponent = null;
            IQueryable<Activity> activities = db.Activities.Where(x =>
            (x.Target.Component == stepComponent || stepComponent == null) &&
             (acceptableId == null ||
              (db.Steps.Where(step =>
               db.StepGroups.Where(stepGroup =>
               db.Inventories.Where(group =>
               db.Relationships.Where(inventoryGroup =>
               inventoryGroup.OwnerId == acceptableId)
               .Any(inventoryGroup => inventoryGroup.TargetId == group.Id))
               .Any(group => group.Id == stepGroup.TargetId))
               .Any(stepGroup => stepGroup.OwnerId == step.Id))
               .Any(step => step.Id == x.TargetId) &&
               (db.Participates.Where(p => p.OwnerId == x.Id && p.ParticipantId == acceptableId).Count() == 0) &&
               (x.Id == x.Owner.CurrentId)
              )
             ) &&
             (ownerId == null || x.OwnerId == ownerId)
            );
            switch (sortBy)
            {
                default:
                    if (orderBy == "asc") return Ok(new Query<Activity>
                    {
                        PaginatorLength = activities.Count(),
                        Data = activities.OrderBy(x => x.Target.Sort).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<Activity>()
                    });
                    else return Ok(new Query<Activity>
                    {
                        PaginatorLength = activities.Count(),
                        Data = activities.OrderByDescending(x => x.Target.Sort).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<Activity>()
                    });
            }
        }

        public async Task<IHttpActionResult> PostActivity(Activity activity)
        {
            db.Activities.Add(activity);
            await db.SaveChangesAsync();
            await db.Entry(activity).GetDatabaseValuesAsync();
            return Ok(activity);
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