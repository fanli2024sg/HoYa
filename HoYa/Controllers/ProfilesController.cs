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
   // [Authorize]
    public class ProfilesController : ApiController
    {
        private HoYaContext db = new HoYaContext();
   
        public async Task<IHttpActionResult> GetProfile(Guid id)
        {
            Profile profile = await db.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        
        public async Task<IHttpActionResult> GetProfiles(
            string anyLike = "",
            string sortBy = "",
            string orderBy = "",
            int? pageIndex = 1,
            int? pageSize = 20
        )
        {
            IQueryable<Profile> profiles = db.Profiles.Where(x => (x.Definition.Value.Contains(anyLike) || anyLike == null) ||
                           (x.Definition.Code.Contains(anyLike) || anyLike == null) ||
                           (x.No.Contains(anyLike) || anyLike == null) ||
                           (x.Value.Contains(anyLike) || anyLike == ""));
            switch (sortBy)
            {
                 default:
                    if (orderBy == "asc") return Ok(new Query<Profile>
                    {
                        PaginatorLength = profiles.Count(),
                        Data = profiles.OrderBy(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<Profile>()
                    });
                    else return Ok(new Query<Profile>
                    {
                        PaginatorLength = profiles.Count(),
                        Data = profiles.OrderByDescending(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<Profile>()
                    });
            }
        }

        public async Task<IHttpActionResult> PutProfile(Guid id, Profile profile)
        {
            Profile existedProfile = await db.Profiles.Where(p => p.Id == id).AsQueryable().FirstOrDefaultAsync();
           
            db.Entry(existedProfile).CurrentValues.SetValues(profile);
            await db.SaveChangesAsync();
            return Ok(existedProfile);
        }

        [Route("api/Profiles/Groups")]
        public async Task<IHttpActionResult> PutProfileGroup(Guid id, ProfileGroup profileGroup)
        {
            ProfileGroup existedProfileGroup = await db.ProfileGroups.FindAsync(id);

            db.Entry(existedProfileGroup).CurrentValues.SetValues(profileGroup);
            await db.SaveChangesAsync();
            return Ok(existedProfileGroup);
        }

        [Route("api/Profiles/Groups")]
        public async Task<IHttpActionResult> PostProfileGroup(ProfileGroup profileGroup)
        {
            db.ProfileGroups.Add(profileGroup);
            await db.SaveChangesAsync();
            await db.Entry(profileGroup).GetDatabaseValuesAsync();
            return Ok(profileGroup);
        }

        [Route("api/Profiles/Groups")]
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
                           (x.OwnerId == ownerId || ownerId == null)
                           ); 
        }

        public async Task<IHttpActionResult> PostProfile(Profile profile)
        {
            db.Profiles.Add(profile);
            await db.SaveChangesAsync();
            await db.Entry(profile).GetDatabaseValuesAsync();
            return Ok(profile);
        }
        public async Task<IHttpActionResult> DeleteProfile(Guid id)
        {
            Profile profile = await db.Profiles.FindAsync(id);
            if (profile == null) return NotFound();
            db.Profiles.Remove(profile);
            await db.SaveChangesAsync();
            return Ok(profile);
        }
    }

}