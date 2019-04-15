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
    public class MaterialsController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        [Route("api/Materials")]
        [ResponseType(typeof(Material))]
        public IQueryable<Material> GetMaterialsBy(string anyLike="")
        {
            if (anyLike == null) anyLike = "";
            return db.Materials.Where(x => x.Value.Contains(anyLike));
        }

        [ResponseType(typeof(Material))]
        public async Task<IHttpActionResult> PostMaterial(Material material)
        {
            db.Materials.Add(material);
            await db.SaveChangesAsync();
            await db.Entry(material).GetDatabaseValuesAsync();
            return Ok(material);
        }
    }
}