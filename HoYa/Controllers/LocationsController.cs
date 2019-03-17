using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Security.Claims;
using System;

namespace HoYa.Controllers
{
    //[Authorize]

    public class LocationsController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public IQueryable<Location> GetLocations()
        {
            return db.Locations;
        }
        [Route("api/Locations/ByGroup")]
        [ResponseType(typeof(Location))]
        public IQueryable<Location> GetLocationsByGroup(Guid id)
        {
            return db.Locations.Where(option => option.ParentId == id);
        }

        [ResponseType(typeof(Location))]
        public async Task<IHttpActionResult> GetLocation(Guid id)
        {
            Location existedLocation = await db.Locations.FindAsync(id);
            if (existedLocation == null) return NotFound();
            return Ok(existedLocation);
        }

        [Route("api/Locations/By")]
        public IQueryable<Location> GetLocationByValue(string value="")
        {
            if (value == null) value = "";

            return db.Locations.Where(x=>x.Value.Contains(value)).OrderBy(x => x.Code).Take(6).OrderBy(x=>x.Value);
          
        }

        [ResponseType(typeof(Location))]
        public async Task<IHttpActionResult> PutLocation(Guid id, Location option)
        {
            Location existedLocation = await db.Locations.FindAsync(id);
            db.Entry(existedLocation).CurrentValues.SetValues(option);
            await db.SaveChangesAsync();
            await db.Entry(existedLocation).GetDatabaseValuesAsync();
            return Ok(existedLocation);
        }

        [ResponseType(typeof(Location))]
        public async Task<IHttpActionResult> PostLocation(Location option)
        {
            option.Id = Guid.NewGuid();
            db.Locations.Add(option);
            await db.SaveChangesAsync();
            await db.Entry(option).GetDatabaseValuesAsync();
            return Ok(option);
        }

        public async Task<IHttpActionResult> DeleteLocation(Guid id)
        {
            Location existedLocation = await db.Locations.FindAsync(id);
            if (existedLocation == null) return NotFound();
            db.Locations.Remove(existedLocation);
            await db.SaveChangesAsync();
            return Ok();
        }

    }
}