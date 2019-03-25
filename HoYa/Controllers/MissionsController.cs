using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Web.Http.Description;
using System;

namespace HoYa.Controllers
{
    //[Authorize]

    public class MissionsController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public IQueryable<Mission> GetMissions()
        {
            return db.Missions;
        }
        [Route("api/Missions/By")]
        [ResponseType(typeof(Mission))]
        public IQueryable<Mission> GetMissionsBy(
            Guid? definitionDetailId = null,
            string anyLike = "")
        {
            return db.Missions.Where(x => (x.DefinitionDetailId == definitionDetailId || definitionDetailId == null));
        }

        [ResponseType(typeof(Mission))]
        public async Task<IHttpActionResult> GetMission(Guid id)
        {
            Mission existedMission = await db.Missions.FindAsync(id);
            if (existedMission == null) return NotFound();
            return Ok(existedMission);
        }

        [ResponseType(typeof(Mission))]
        public async Task<IHttpActionResult> PutMission(Guid id, Mission mission)
        {
            Mission existedMission = await db.Missions.FindAsync(id);
            mission.Id = existedMission.Id;
            db.Entry(existedMission).CurrentValues.SetValues(mission);
            await db.SaveChangesAsync();
            await db.Entry(existedMission).GetDatabaseValuesAsync();
            return Ok(existedMission);
        }

        [ResponseType(typeof(Mission))]
        public async Task<IHttpActionResult> PostMission(Mission mission)
        {
            db.Missions.Add(mission);
            await db.SaveChangesAsync();
            await db.Entry(mission).GetDatabaseValuesAsync();
            return Ok(mission);
        }

        public async Task<IHttpActionResult> DeleteMission(Guid id)
        {
            Mission existedMission = await db.Missions.FindAsync(id);
            if (existedMission == null) return NotFound();
            db.Missions.Remove(existedMission);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}