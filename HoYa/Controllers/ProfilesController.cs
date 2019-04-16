using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System;
using System.Web.Http.Description;

namespace HoYa.Controllers
{
   // [Authorize]
    public class ProfilesController : ApiController
    {
        private HoYaContext db = new HoYaContext();
        public IQueryable<Profile> GetProfiles()
        {
            return db.Profiles;
        }
        public async Task<IHttpActionResult> GetProfile(Guid id)
        {
            Profile profile = await db.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        [Route("api/Profiles/By")]
        [ResponseType(typeof(Profile))]
        public async Task<IHttpActionResult> GetProfileBy(string userId)
        {
            Profile profile = await db.Profiles.FirstOrDefaultAsync(x=>x.UserId==userId);
            if (profile == null) return NotFound();
            return Ok(profile);
        }

        public async Task<IHttpActionResult> PutProfile(Guid id, Profile profile)
        {
            Profile existedProfile = await db.Profiles.Where(p => p.Id == id).AsQueryable().FirstOrDefaultAsync();
           
            db.Entry(existedProfile).CurrentValues.SetValues(profile);
            await db.SaveChangesAsync();
            return Ok(existedProfile);
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