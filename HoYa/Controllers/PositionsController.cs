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
using System.Web;

namespace HoYa.Controllers
{
    [Authorize]
    public class PositionsController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public IQueryable<Position> GetPositions(
            Guid? ownerId = null)
        {
            return db.Positions.Where(x => x.OwnerId == ownerId || ownerId == null);
        } 

        public async Task<IHttpActionResult> GetPosition(Guid id)
        {
            Position position = await db.Positions.FindAsync(id);
            if (position == null)
            {
                return Ok();
            }
            return Ok(position);
        }

        public async Task<IHttpActionResult> PostPosition(Position position)
        {
            Position prePosition = db.Positions.Where(x => x.OwnerId == position.OwnerId).OrderBy(x => x.StartDate).FirstOrDefault();
            if(prePosition!=null) position.PreOwnerId = prePosition.TargetId;
            db.Positions.Add(position);
            await db.SaveChangesAsync();
            Inventory inventory = await db.Inventories.FindAsync(position.OwnerId);
            inventory.PositionId = position.Id;
            Inventory existedInventory = await db.Inventories.FindAsync(inventory.Id);
            db.Entry(existedInventory).CurrentValues.SetValues(inventory);
            await db.SaveChangesAsync();
            string userId = HttpContext.Current.User.Identity.Name;
            Guid archivedById = (await db.Inventories.FirstOrDefaultAsync(x => x.UserId == userId)).Id;
            if (prePosition != null)
            {
                Position existedPrePosition = await db.Positions.FindAsync(prePosition.Id);
                prePosition.EndDate = position.StartDate;
                prePosition.ArchivedById = archivedById;
                prePosition.ArchivedDate = prePosition.EndDate;
                db.Entry(existedPrePosition).CurrentValues.SetValues(prePosition);
            }
            await db.SaveChangesAsync();
            return Ok(position);
        }


        public async Task<IHttpActionResult> PutPosition(Guid id, Position position)
        {
            Position existedPosition = await db.Positions.FindAsync(id);
            db.Entry(existedPosition).CurrentValues.SetValues(position);
            await db.SaveChangesAsync();
            return Ok(existedPosition);
        }

        public async Task<IHttpActionResult> DeletePosition(Guid id)
        {
            Position existedPosition = await db.Positions.FindAsync(id);
            if (existedPosition != null) db.Positions.Remove(existedPosition);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}