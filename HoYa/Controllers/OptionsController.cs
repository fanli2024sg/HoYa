using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using HoYa.Models;

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
            if (anyLike == null) anyLike = "";
            return db.Options.Where(x => (x.Value.ToString().Contains(anyLike) ||
            x.Code.ToString().Contains(anyLike)) &&
            (x.ParentId == parentId || parentId == null)
            ).OrderBy(x => x.Code);
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
            option.Status = await db.Options.FindAsync(option.StatusId);
            Option existedOption = await db.Options.FirstOrDefaultAsync(x => option.CreatedById == x.CreatedById && x.Status.Code == "statusC");
            if (existedOption == null)
            {
                db.Options.Add(option);
                await db.SaveChangesAsync();
                await db.Entry(option).GetDatabaseValuesAsync();              
                return Ok(option);
            }
            return Ok(existedOption);
        }

        [Route("api/Options/Count")]
        public async Task<IHttpActionResult> GetOptionsCount(
           string value = "",
           string code = "",
           Guid? statusId = null,
           Guid? excludeId = null,
           Guid? createdById = null,
           Guid? parentId = null
       )
        {
            Guid exclude = excludeId.GetValueOrDefault();
            if (value == null) value = "";
            if (code == null) code = "";

            return Ok(db.Options.Where(x =>
            (x.Id != exclude || excludeId == null) &&
            (x.ParentId == parentId || parentId == null) &&
            (x.StatusId == statusId || statusId == null) &&
            (x.Value == value || value == "") &&
            (x.Code == code || code == "") &&
            (x.CreatedById == createdById || createdById == null) &&
            (!(value == "" && code == "") || excludeId == null)
            ).Count());
        }

        [Route("api/Options/By")]
        public async Task<IHttpActionResult> GetOptionBy(
            string value = "",
            string code = "",
            Guid? parentId = null
        )
        {
            if (value == null) value = "";
            if (code == null) code = "";
            return Ok(await db.Options.FirstOrDefaultAsync(x =>
            (x.ParentId == parentId || parentId == null) &&
            (x.Value == value || value == "") &&
            (x.Code == code || code == "")
            ));
        }

        public async Task<IHttpActionResult> DeleteOption(Guid id)
        {
            Option existedOption = await db.Options.FindAsync(id);
            if (existedOption == null) return NotFound();
            db.Options.Remove(existedOption);
            await db.SaveChangesAsync();
            return Ok();
        }
        [Route("api/Options/Select")]
        public IQueryable<OptionSelect> GetOptionValueSelects(
                 string valueLike = "",
            string codeLike = "",
            Guid? parentId = null,
            Guid? statusId = null
        )
        {
            if (valueLike == null) valueLike = "";
            if (codeLike == null) codeLike = "";
            return db.Options.Where(option =>
            (option.ParentId == parentId || parentId == null) &&
            (option.StatusId == statusId || statusId == null) &&
            option.Value.ToString().Contains(valueLike) &&
            option.Code.ToString().Contains(codeLike)
            ).Select(option => new OptionSelect
            {
                Id = option.Id,
                Code = option.Code,
                Value = option.Value
            }).OrderBy(x => x.Value);
        }
    }
}