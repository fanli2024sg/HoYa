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
using System.Web;

namespace HoYa.Controllers
{
    //[Authorize]

    public class ExchangesController : ApiController
    {
        private HoYaContext db = new HoYaContext();
        public async Task<IHttpActionResult> GetExchanges(
           string anyLike = "",
           int? take = 200,
           Guid? ownerId = null
       )
        {
            if (anyLike == null) anyLike = "";
            else anyLike = HttpUtility.UrlDecode(anyLike);
            IQueryable<Exchange> exchanges = null;

            if (anyLike != "")
            {
                if (ownerId != null) exchanges = db.Exchanges.Where(x => x.OwnerId == ownerId && x.Target.No.Contains(anyLike));
                exchanges = db.Exchanges.Where(x => x.Target.No.Contains(anyLike)); 
            }
            else
            {
                if (ownerId != null) exchanges = db.Exchanges.Where(x => x.OwnerId == ownerId);
                else exchanges = db.Exchanges;
            }
          
            return Ok(exchanges.Take(take.GetValueOrDefault()).Select(x => new { id = x.TargetId, no = x.Target.No, value = x.Value }));
        }
    }
}