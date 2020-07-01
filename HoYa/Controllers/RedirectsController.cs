using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Web.Http.Description;
using System;
using System.Web;
using HoYa.Models;

namespace HoYa.Controllers
{
    [Authorize]
    public class RedirectsController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public async Task<IHttpActionResult> GetRedirect(Guid id)
        {
            Redirect existedRedirect = await db.Redirects.FindAsync(id);
            if (existedRedirect == null) return NotFound();
            return Ok(existedRedirect);
        }

        public async Task<IHttpActionResult> PutRedirect(Guid id, Redirect condition)
        {
            Redirect existedRedirect = await db.Redirects.FindAsync(id);
        
            db.Entry(existedRedirect).CurrentValues.SetValues(condition);
            await db.Entry(existedRedirect).GetDatabaseValuesAsync();
            return Ok(existedRedirect);
        }




        public IQueryable<Redirect> GetRedirects(Guid? ownerId = null)
        {
            return db.Redirects.Where(x => 
            
            (x.OwnerId == ownerId || ownerId == null)
            
            );
        }



        [Route("api/Redirects/Query")]
        public async Task<IHttpActionResult> GetRedirectsQuery(
         string anyLike = "",
        Guid? ownerId = null,
           Guid? participantId = null,
         string sortBy = "",
         string orderBy = "",
         int? pageIndex = 1,
         int? pageSize = 20
     )
        {
            IQueryable<Redirect> redirects = db.Redirects.Where(x =>
             (x.OwnerId == ownerId || ownerId == null));
            switch (sortBy)
            {
                default:
                    if (orderBy == "asc") return Ok(new Query<Redirect>
                    {
                        PaginatorLength = redirects.Count(),
                        Data = redirects.OrderBy(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<Redirect>()
                    });
                    else return Ok(new Query<Redirect>
                    {
                        PaginatorLength = redirects.Count(),
                        Data = redirects.OrderByDescending(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<Redirect>()
                    });
            }
        }

        public async Task<IHttpActionResult> PostRedirect(Redirect redirect)
        {
            db.Redirects.Add(redirect);
            await db.SaveChangesAsync();
            await db.Entry(redirect).GetDatabaseValuesAsync();
            return Ok(redirect);
        }

        public async Task<IHttpActionResult> DeleteRedirect(Guid id)
        {
            Redirect existedRedirect = await db.Redirects.FindAsync(id);
            if (existedRedirect == null) return NotFound();
            db.Redirects.Remove(existedRedirect);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}