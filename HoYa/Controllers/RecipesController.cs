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

namespace HoYa.Controllers
{
    public class RecipesController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        [ResponseType(typeof(Recipe))]
        public IQueryable<Recipe> GetRecipesBy(string anyLike="")
        {
            if (anyLike == null) anyLike = "";
            return db.Recipes.Where(x => x.Value.Contains(anyLike));
        }

        [ResponseType(typeof(Recipe))]
        public async Task<IHttpActionResult> PostMaterial(Recipe material)
        {
            db.Recipes.Add(material);
            await db.SaveChangesAsync();
            await db.Entry(material).GetDatabaseValuesAsync();
            return Ok(material);
        }
    }
}