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
using System.Security.Claims;
using System.Web;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;

namespace HoYa.Controllers
{ 
    public class SearchController : ApiController
    {
        private HoYaContext db = new HoYaContext(); 

        public async Task<IHttpActionResult> GetSearch(string anyLike)
        {
            if (anyLike == null) anyLike = "";
            else anyLike = HttpUtility.UrlDecode(anyLike);
            return Ok(
              db.Categories.Where(x =>
                (("#" + x.Code).ToString().Contains(anyLike) || x.Code.ToString().Contains(anyLike) || anyLike == null) ||
                (("#" + x.Value).ToString().Contains(anyLike) || x.Value.ToString().Contains(anyLike) || anyLike == null) ||
                (db.ItemCategories.Where(itemCategory => itemCategory.Target.Value.ToString().Contains(anyLike) && itemCategory.OwnerId == x.Id && itemCategory.EndDate == null).Count() > 0)
                ).Select(category => new 
                {
                   navigateUrl = category.Id.ToString(),
                    photoPath = "",
                    title = "#" + category.Value,
                    subTitle = db.Items.Where(item => (db.ItemCategories.Where(itemCategory => itemCategory.TargetId == category.Id && itemCategory.OwnerId == item.Id && itemCategory.EndDate == null).Count() > 0)).Count().ToString() + "筆存量",
                    type = "categories"
                }).Union(
                db.Inventories.Where(x =>
                 x.No.ToString() != "" &&
                (x.No.ToString().Contains(anyLike) || anyLike == null) ||
                (db.ItemCategories.Where(itemCategory => itemCategory.Target.Value.ToString().Contains(anyLike) && itemCategory.OwnerId == x.ItemId && itemCategory.EndDate == null).Count() > 0)
                ).Select(x => new 
                {
                    navigateUrl = x.Id.ToString(),
                    photoPath = x.Photo==null?"": x.Photo.Target.Path,
                    title = x.No,
                    subTitle = db.Inventories.Where(inventory => (db.Positions.Where(position => position.OwnerId == inventory.Id && position.TargetId == x.Id && position.EndDate == null).Count() > 0)).Count().ToString() + "筆內容物",

                    type = "inventories"
                })).Union(
                db.Items.Where(x =>
                (x.Code.ToString().Contains(anyLike) || anyLike == null) ||
                (x.Value.ToString().Contains(anyLike) || anyLike == null) ||
                (db.ItemCategories.Where(itemCategory => itemCategory.Target.Value.ToString().Contains(anyLike) && itemCategory.OwnerId == x.Id && itemCategory.EndDate == null).Count() > 0)
                ).Select(x => new 
                {
                    navigateUrl = x.Id.ToString(),
                    photoPath = x.Photo == null ? "" : x.Photo.Target.Path,
                    title = x.Value,
                    subTitle = db.Inventories.Where(inventory => inventory.ItemId == x.Id).Count().ToString() + "筆存量",
                    type = "items"
                })).ToList()
            );
        }

        [Route("api/Search/Conditions")]
        public async Task<IHttpActionResult> GetSearchConditions(string anyLike, int? take = 5)
        {
            if (anyLike == null) anyLike = "";
            else anyLike = HttpUtility.UrlDecode(anyLike);

            return Ok(
              db.Categories.Where(x =>
                (("#" + x.Code).ToString().Contains(anyLike) || x.Code.ToString().Contains(anyLike) || anyLike == null) ||
                (("#" + x.Value).ToString().Contains(anyLike) || x.Value.ToString().Contains(anyLike) || anyLike == null) ||
                (db.ItemCategories.Where(itemCategory => itemCategory.Target.Value.ToString().Contains(anyLike) && itemCategory.OwnerId == x.Id && itemCategory.EndDate == null).Count() > 0)
                ).Select(category => new 
                {
                    id = category.Id,
                    value = "#" + category.Value,
                    desc = "",
                    type = "category"
                }).Union(
                db.Items.Where(x =>
                (x.Code.ToString().Contains(anyLike) || anyLike == null) ||
                (x.Value.ToString().Contains(anyLike) || anyLike == null) ||
                (db.ItemCategories.Where(itemCategory => itemCategory.Target.Value.ToString().Contains(anyLike) && itemCategory.OwnerId == x.Id && itemCategory.EndDate == null).Count() > 0)
                ).Select(x => new 
                {
                    id = x.Id,
                    value = x.Value,
                    desc = x.Code,
                    type = "item"
                })).Union(
                db.Inventories.Where(x =>
                 x.No.ToString() != "" &&
                (x.No.ToString().Contains(anyLike) || anyLike == null) ||
                (db.ItemCategories.Where(itemCategory => itemCategory.Target.Value.ToString().Contains(anyLike) && itemCategory.OwnerId == x.ItemId && itemCategory.EndDate == null).Count() > 0)
                ).Select(x => new
                {
                    id = x.Id,
                    value = x.No,
                    desc = "",
                    type = "inventory"
                })).Take(5).ToList()
            );
        }
    }
}