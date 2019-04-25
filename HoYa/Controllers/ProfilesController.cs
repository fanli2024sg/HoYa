using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System;
using System.Web.Http.Description;
using HoYa.Models; 
namespace HoYa.Controllers
{
    [Authorize]
    public class ProfileGroupsController : ApiController
    {
        private HoYaContext db = new HoYaContext();   
    
        public async Task<IHttpActionResult> PutProfileGroup(Guid id, ProfileGroup profileGroup)
        {
            ProfileGroup existedProfileGroup = await db.ProfileGroups.FindAsync(id);

            db.Entry(existedProfileGroup).CurrentValues.SetValues(profileGroup);
            await db.SaveChangesAsync();
            return Ok(existedProfileGroup);
        }


        public async Task<IHttpActionResult> PostProfileGroup(ProfileGroup profileGroup)
        {
            db.ProfileGroups.Add(profileGroup);
            await db.SaveChangesAsync();
            await db.Entry(profileGroup).GetDatabaseValuesAsync();
            return Ok(profileGroup);
        }

        public IQueryable<ProfileGroup> GetProfileGroups(
            string anyLike = "",
            Guid? ownerId = null,

            string sortBy = "",
            string orderBy = "",
            int? pageIndex = 1,
            int? pageSize = 20
        )
        {
            return db.ProfileGroups.Where(x => ((x.Target.Value.Contains(anyLike) || anyLike == null) ||
                           (x.Target.Code.Contains(anyLike) || anyLike == null)) &&
                           (x.OwnerId == ownerId || ownerId == null) &&
                           x.ArchivedDate ==null
                           ); 
        }
    }
}