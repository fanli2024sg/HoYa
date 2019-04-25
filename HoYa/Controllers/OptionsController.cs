using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Security.Claims;
using System;
using System.Collections.Generic;

namespace HoYa.Controllers
{
    //[Authorize]

    public class OptionsController : ApiController
    {
        private HoYaContext db = new HoYaContext();

    
      
        public IQueryable<Option> GetOptions(
         string anyLike = "",
         Guid? parentId = null,

         string sortBy = "",
         string orderBy = "",
         int? pageIndex = 1,
         int? pageSize = 20
     )
        {
            return db.Options.Where(x => ((x.Value.Contains(anyLike) || anyLike == null) ||
                           (x.Code.Contains(anyLike) || anyLike == null)) &&
                           (x.ParentId == parentId || parentId == null)
                           );
        }

        [ResponseType(typeof(Option))]
        public async Task<IHttpActionResult> GetOption(Guid id)
        {
            Option existedOption = await db.Options.FindAsync(id);
            if (existedOption == null) return NotFound();
            return Ok(existedOption);
        }

        [ResponseType(typeof(Option))]
        public async Task<IHttpActionResult> PutOption(Guid id, Option option)
        {
            Option existedOption = await db.Options.FindAsync(id);
            option.Id = existedOption.Id;
            db.Entry(existedOption).CurrentValues.SetValues(option);
            await db.SaveChangesAsync();
            await db.Entry(existedOption).GetDatabaseValuesAsync();
            return Ok(existedOption);
        }

        [ResponseType(typeof(Option))]
        public async Task<IHttpActionResult> PostOption(Option option)
        {
            option.Id = Guid.NewGuid();
            option.Parent = await db.Options.FindAsync(option.ParentId);
            option.CreatedDate = DateTime.Now;
            db.Options.Add(option);
            await db.SaveChangesAsync();
            await db.Entry(option).GetDatabaseValuesAsync();
            return Ok(option);
        }

        public async Task<IHttpActionResult> DeleteOption(Guid id)
        {
            Option existedOption = await db.Options.FindAsync(id);
            if (existedOption == null) return NotFound();
            db.Options.Remove(existedOption);
            await db.SaveChangesAsync();
            return Ok();
        }

    }
}