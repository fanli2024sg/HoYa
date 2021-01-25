using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using HoYa.Models;
using System.Web;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using Newtonsoft.Json.Linq;

namespace HoYa.Controllers
{
    //[Authorize]



    public class WorkPlansController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public async Task<IHttpActionResult> GetWorkPlan(Guid id)
        {
            IQueryable<WorkPlan> workPlans = db.WorkPlans.Where(x => x.Id == id);


            if (workPlans.Count() == 0)
            {
                return NotFound();
            }

            return Ok(workPlans.Select(x => new
            {
                id = x.Id,
                startDate=x.StartDate,
                no = x.No,
                recipeNo= x.Item.Recipe.No,
                recipeId = x.Item.RecipeId,
                value =x.WorkPlanRecord.Value,
                unitValue= x.WorkPlanRecord.Unit.Value
            }).FirstOrDefault());
        }


        public async Task<IHttpActionResult> GetWorkPlans(
            string anyLike = "",
            int? take = 200,
            Guid? itemId = null
        )
        {
            if (anyLike == null) anyLike = "";
            else anyLike = HttpUtility.UrlDecode(anyLike);
            IQueryable<WorkPlan> workPlans = null;

            if (anyLike != "")
            {
                if (itemId != null) workPlans = db.WorkPlans.Where(x => x.ItemId == itemId && x.WorkPlanRecord.Value.ToString().Contains(anyLike));
                workPlans = db.WorkPlans.Where(x => x.WorkPlanRecord.Value.ToString().Contains(anyLike));
            }
            else
            {
                if (itemId != null) workPlans = db.WorkPlans.Where(x => x.ItemId == itemId);
                else workPlans = db.WorkPlans;
            }

            return Ok(workPlans.Take(take.GetValueOrDefault()).Select(x => new { id = x.Id, value = x.WorkPlanRecord.Value, unitNo = x.WorkPlanRecord.Unit.No }));
        }

        [Route("api/WorkOrders")]
        public async Task<IHttpActionResult> PostWorkPlan(DTOWorkPlan dto)
        {

            WorkPlan workPlan = new WorkPlan
            {
                ItemId = dto.ItemId,
                WorkPlanRecord = new WorkPlanRecord
                {
                    Value = dto.Value,
                    UnitId = dto.UnitId
                }
            };
            db.WorkPlans.Add(workPlan);
            await db.SaveChangesAsync();
            Item item = await db.Items.FindAsync(dto.ItemId);

           int sum = await InsertWorkOrder(workPlan.Id.ToString(),item,1);
            return Ok(sum);
        }

        private async Task<int> InsertWorkOrder(string workPlanId,Item item, decimal? value)
        {
            WorkOrder workOrder = new WorkOrder(workPlanId)
            {
                ItemId = item.Id,
                Value = value
            };
            db.WorkOrders.Add(workOrder);

            try
            {
              
                await db.SaveChangesAsync();
                Recipe recipe = await db.Recipes.FindAsync(item.RecipeId);

                int sum = 0;
                foreach (var input in db.Inputs.Where(x => x.OwnerId == recipe.Id))
                {
                   sum =sum + await InsertWorkOrder(workPlanId, input.Item, input.Value);
                }
                return sum+1;
            }
            catch(Exception e)
            {
                return 0;
            }
        }
    }
}