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
    public class MaterialProceduresController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        [Route("api/MaterialProcedures/By")]
        [ResponseType(typeof(MaterialProcedure))]
        public IQueryable<MaterialProcedure> GetMaterialProceduresBy(string anyLike="")
        {
            if (anyLike == null) anyLike = "";
            return db.MaterialProcedures.Where(x => x.Value.Contains(anyLike));
        }

        [ResponseType(typeof(MaterialProcedure))]
        public async Task<IHttpActionResult> PostMaterialProcedure(MaterialProcedure materialProcedure)
        {
            db.MaterialProcedures.Add(materialProcedure);
            await db.SaveChangesAsync();
            await db.Entry(materialProcedure).GetDatabaseValuesAsync();
            return Ok(materialProcedure);
        }
    }
}