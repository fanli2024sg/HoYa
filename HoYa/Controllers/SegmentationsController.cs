using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Web.Http.Description;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using HoYa.Models;
using HoYa.Repository;
using System.Security.Claims;

namespace HoYa.Controllers
{
    [Authorize]
    public class SegmentationsController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public IQueryable<Segmentation> GetSegmentations(
            Guid? ownerId = null)
        {
            return db.Segmentations.Where(x => x.OwnerId == ownerId || ownerId == null);
        }    

        public async Task<IHttpActionResult> PostSegmentation(Segmentation segmentation)
        {
            segmentation.Owner = await db.Inventories.FindAsync(segmentation.OwnerId);
            db.Segmentations.Add(segmentation);
            await db.SaveChangesAsync();
            return Ok(segmentation);
        }

        [Route("api/Segmentations/Count")]
        public async Task<IHttpActionResult> GetItemsCount(
            Guid? ownerId = null
        )
        {
            return Ok(db.Segmentations.Where(x =>
            (x.OwnerId== ownerId || ownerId == null)
            ).Count());
        }


        public async Task<IHttpActionResult> PutSegmentation(Guid id, Segmentation segmentation)
        {
            Segmentation existedSegmentation = await db.Segmentations.FindAsync(id);
            db.Entry(existedSegmentation).CurrentValues.SetValues(segmentation);
            await db.SaveChangesAsync();
            return Ok(existedSegmentation);
        }

        public async Task<IHttpActionResult> DeleteSegmentation(Guid id)
        {
            Segmentation existedSegmentation = await db.Segmentations.FindAsync(id);
            if(existedSegmentation!=null) db.Segmentations.Remove(existedSegmentation); 
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}