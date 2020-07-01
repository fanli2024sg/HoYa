using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Web.Http.Description;
using System.Data.Entity;
using System;

namespace HoYa.Controllers
{
    [Authorize]
    public class RolesController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        /// <summary>  
        /// 取得所有【角色】
        /// </summary>
        public IQueryable<AspNetRole> GetRoles()
        {
            return db.AspNetRoles;
        }

        /// <summary>  
        /// 取得【角色】
        /// </summary>  
        /// <param name="id">角色序號</param>
        [ResponseType(typeof(AspNetRole))]
        public async Task<IHttpActionResult> GetRole(string id)
        {
            AspNetRole role = await db.AspNetRoles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        /// <summary>  
        /// 更新【角色】
        /// </summary>  
        /// <param name="id">角色序號</param>  
        /// <param name="role">角色</param>
        public async Task<IHttpActionResult> PutRole(string id, AspNetRole role)
        {
            AspNetRole existedRole = await db.AspNetRoles.Where(a => a.Id == id).AsQueryable().FirstOrDefaultAsync();
            db.Entry(existedRole).CurrentValues.SetValues(role);
            await db.SaveChangesAsync();
            return Ok();
        }

        /// <summary>  
        /// 新增【角色】
        /// </summary>
        /// <param name="role">角色</param>
        public async Task<IHttpActionResult> PostRole(AspNetRole role)
        {
            db.AspNetRoles.Add(role);
            await db.SaveChangesAsync();
            await db.Entry(role).GetDatabaseValuesAsync();
            return Ok(role.Id);
        }

        /// <summary>  
        /// 刪除【角色】
        /// </summary>  
        /// <param name="id">角色序號</param>
        public async Task<IHttpActionResult> DeleteRole(Guid id)
        {
            AspNetRole role = await db.AspNetRoles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            db.AspNetRoles.Remove(role);
            await db.SaveChangesAsync();
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}