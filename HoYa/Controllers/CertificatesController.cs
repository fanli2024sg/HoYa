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

    public class CertificatesController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public IQueryable<Certificate> GetLicenses()
        {
            return db.Certificates;
        }

        [ResponseType(typeof(Certificate))]
        public async Task<IHttpActionResult> GetLicense(Guid id)
        {
            Certificate existedLicense = await db.Certificates.FindAsync(id);
            if (existedLicense == null) return NotFound();
            return Ok(existedLicense);
        }

        [Route("api/Licenses/ByOwnerId")]
        public IQueryable<Certificate> GetLicenseByOwnerId( Guid ownerId)
        {
            return db.Certificates.Where( x=>x.OwnerId== ownerId );    
        }

        [Route("api/Licenses/ByTypeIdAndOwnerId")]
        [ResponseType(typeof(Certificate))]
        public async Task<IHttpActionResult> GetLicenseTypeId(Guid typeId, Guid ownerId)
        {
            Certificate existedLicense = await db.Certificates.Where(x => x.TypeId == typeId && x.OwnerId == ownerId).OrderBy(x=>x.EndDate).FirstOrDefaultAsync();
            if (existedLicense == null) return NotFound();
            return Ok(existedLicense);
        }

        [ResponseType(typeof(Certificate))]
        public async Task<IHttpActionResult> PutLicense(Guid id, Certificate License)
        {
            Certificate existedLicense = await db.Certificates.FindAsync(id);
            db.Entry(existedLicense).CurrentValues.SetValues(License);
            await db.SaveChangesAsync();
            await db.Entry(existedLicense).GetDatabaseValuesAsync();
            return Ok(existedLicense);
        }

        [ResponseType(typeof(Certificate))]
        public async Task<IHttpActionResult> PostLicense(Certificate license)
        {
            license.Id = Guid.NewGuid();
            license.Type = await db.Options.FindAsync(license.TypeId);
            db.Certificates.Add(license);
            await db.SaveChangesAsync();
            await db.Entry(license).GetDatabaseValuesAsync();
            return Ok(license);
        }

        public async Task<IHttpActionResult> DeleteLicense(Guid id)
        {
            Certificate existedLicense = await db.Certificates.FindAsync(id);
            if (existedLicense == null) return NotFound();
            db.Certificates.Remove(existedLicense);
            await db.SaveChangesAsync();
            return Ok();
        }

    }
}