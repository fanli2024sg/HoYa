using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using HoYa.Repository;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Security.Claims;
using System;
using System.Collections.Generic;

namespace HoYa.Controllers
{
    [Authorize]
    public class MenusController : ApiController
    {
        private HoYaContext db = new HoYaContext();
        private HoYaRepository repo = null;

        public MenusController()
        {
            repo = new HoYaRepository(db);
        }


        public IQueryable<Menu> GetMenus()
        {
            return db.Menus;
        }
        [Route("api/Menus/ByParentMenu")]
        [ResponseType(typeof(Menu))]
        public IQueryable<Menu> GetMenusByParentMenu(Guid id)
        {
            var functions = repo.Functions((User as ClaimsPrincipal).Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value);
            return db.Menus.Where(x => x.ParentId == id && functions.Any(function => function.Id == x.FunctionId)).ToList().Select(x => new Menu
            {
                Id = x.Id,
                Value = x.Value,
                FunctionId = x.FunctionId,
                Function = x.Function,
                ParentId = x.ParentId,
                /*
                Menus = x.Menus.Where(y => functions.Any(function => function.Id == y.FunctionId)).ToList().Select(y => new Menu
                {
                    Id = y.Id,
                    Value = y.Value,
                    FunctionId = y.FunctionId,
                    Function = y.Function,
                    ParentId = y.ParentId,
                    Menus = y.Menus.Where(z => functions.Any(function => function.Id == z.FunctionId)).ToList().Select(z => new Menu
                    {
                        Id = z.Id,
                        Value = z.Value,
                        FunctionId = z.FunctionId,
                        Function = z.Function,
                        ParentId = z.ParentId
                    }).ToList()
                }).ToList(),*/
            }).AsQueryable();
        }

        private IEnumerable<Function> GetFunctions()
        {
            string userName = (User as ClaimsPrincipal).Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            return db.Functions.Where(function =>
                       db.FunctionGroups.Where(x =>
                           db.Groups.Where(group =>
                               db.ProfileGroups.Where(y =>
                                   y.OwnerId == db.Profiles.FirstOrDefault(profile =>
                                       profile.UserId == db.AspNetUsers.FirstOrDefault(z => z.UserName == userName)
                                       .Id)
                                   .Id)
                               .Any(d => d.TargetId == group.Id))
                           .Any(group => group.Id == x.OwnerId))
                       .Any(x => x.TargetId == function.Id)).AsEnumerable();
        }

        [ResponseType(typeof(Menu))]
        public async Task<IHttpActionResult> GetMenu(Guid id)
        {
            Menu menu = await db.Menus.FindAsync(id);
            if (menu == null) return NotFound();
            return Ok(menu);
        }


        public async Task<IHttpActionResult> PutMenu(Guid id, Menu menu)
        {
            Menu existedMenu = await db.Menus.FindAsync(id);
            db.Entry(existedMenu).CurrentValues.SetValues(menu);
            await db.SaveChangesAsync();
            await db.Entry(existedMenu).GetDatabaseValuesAsync();
            return Ok(existedMenu);
        }

        [ResponseType(typeof(int))]
        public async Task<IHttpActionResult> PostMenu(Menu menu)
        {
            menu.Id = Guid.NewGuid();
            menu.Function = await db.Functions.FindAsync(menu.FunctionId);
            db.Menus.Add(menu);
            await db.SaveChangesAsync();
            await db.Entry(menu).GetDatabaseValuesAsync();
            return Ok(menu);
        }

        public async Task<IHttpActionResult> DeleteMenu(Guid id)
        {
            Menu existedMenu = await db.Menus.FindAsync(id);
            if (existedMenu == null) return NotFound();
            db.Menus.Remove(existedMenu);
            await db.SaveChangesAsync();
            return Ok();
        }

    }
}