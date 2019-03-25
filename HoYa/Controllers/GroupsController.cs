using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Web.Http.Description;
using System.Data.Entity;
using System;
using HoYa.Models;

namespace HoYa.Controllers
{
    //[Authorize]

    public class GroupsController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public IQueryable<Group> GetGroups()
        {
            return db.Groups;
        }
        [Route("api/Groups/Options")]
        public IQueryable<GroupOption> GetGroupOptions(
            Guid? typeId = null,
            string anyLike = "",
            int? pageIndex = 1,
            int? pageSize = 200)
        {
            return db.Groups.Where(x =>
             (x.TypeId == typeId || typeId == null) &&
             ((x.Value + "(" + x.Code + ")").Contains(anyLike) || anyLike == null || anyLike == "")
             ).OrderBy(x => (x.Value + "(" + x.Code + ")")).Take(pageSize.GetValueOrDefault()).Select(x => new GroupOption
             {
                 Id = x.Id,
                 Value = x.Value + "(" + x.Code + ")"
             });
        }

        [Route("api/Groups/Option/{id}")]
        public async Task<IHttpActionResult> GetGroupOption(
           Guid? id = null)
        {
            GroupOption groupOption = await db.Groups.Where(x => x.Id == id).Select(x => new GroupOption
            {
                Id = x.Id,
                Value = x.Value + "(" + x.Code + ")"
            }).FirstOrDefaultAsync();
            return Ok(groupOption);
        }

        [ResponseType(typeof(Group))]
        public async Task<IHttpActionResult> GetGroup(Guid id)
        {
            Group group = await db.Groups.FindAsync(id);

            if (group == null) return NotFound();
            return Ok(group);
        }

        public async Task<IHttpActionResult> PutGroup(Guid id, Group group)
        {
            Group existedGroup = await db.Groups.FindAsync(id);
            db.Entry(existedGroup).CurrentValues.SetValues(group);
            await db.SaveChangesAsync();
            await db.Entry(existedGroup).GetDatabaseValuesAsync();
            return Ok(existedGroup);
        }

        [ResponseType(typeof(int))]
        public async Task<IHttpActionResult> PostGroup(Group group)
        {
            group.Id = Guid.NewGuid();
            db.Groups.Add(group);
            await db.SaveChangesAsync();
            await db.Entry(group).GetDatabaseValuesAsync();
            return Ok(group);
        }

        public async Task<IHttpActionResult> DeleteGroup(Guid id)
        {
            Group group = await db.Groups.FindAsync(id);
            if (group == null) return NotFound();
            db.Groups.Remove(group);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}