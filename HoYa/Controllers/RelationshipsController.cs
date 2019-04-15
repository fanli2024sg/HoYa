using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Web.Http.Description;
using System.Data.Entity;
using System;

namespace HoYa.Controllers
{
    public class RelationshipsController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public IQueryable<Relationship> GetRelationships()
        {
            return db.Relationships;
        }


        [Route("api/Relationships/ByOwner")]
        [ResponseType(typeof(Relationship))]
        public IQueryable<Relationship> GetRelationshipsByOwner(Guid id)
        {
            return db.Relationships.Where(x => x.OwnerId == id);
        }
        [ResponseType(typeof(Relationship))]
        public async Task<IHttpActionResult> GetRelationship(Guid id)
        {
            Relationship relationship = await db.Relationships.FindAsync(id);
            if (relationship == null) return NotFound();
            return Ok(relationship);
        }

        public async Task<IHttpActionResult> PutRelationship(Guid id, Relationship relationship)
        {
            Relationship existedRelationship = await db.Relationships.Where(i => i.Id == id).AsQueryable().FirstOrDefaultAsync();
            db.Entry(existedRelationship).CurrentValues.SetValues(relationship);
            await db.SaveChangesAsync();
            await db.Entry(existedRelationship).GetDatabaseValuesAsync();
            return Ok(existedRelationship);
        }

        [ResponseType(typeof(int))]
        public async Task<IHttpActionResult> PostRelationship(Relationship relationship)
        {
            relationship.Id = Guid.NewGuid();
            db.Relationships.Add(relationship);
            await db.SaveChangesAsync();
            await db.Entry(relationship).GetDatabaseValuesAsync();
            return Ok(relationship);
        }

        public async Task<IHttpActionResult> DeleteRelationship(Guid id)
        {
            Relationship relationship = await db.Relationships.FindAsync(id);
            if (relationship == null) return NotFound();
            db.Relationships.Remove(relationship);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}