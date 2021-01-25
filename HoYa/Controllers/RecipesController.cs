using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using HoYa.Models;

namespace HoYa.Controllers
{
    //[Authorize]
    public class RecipesController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public async Task<IHttpActionResult> PostRecipe(RecipeOption recipeOption)
        {
            Inventory recipe = new Inventory("520934b7-82ed-457e-992f-1bb0cfd3749f") {
                No= recipeOption.Item.Value
            };
            db.Inventories.Add(recipe);
            InventoryAttribute mainOutput = new InventoryAttribute("1873a981-eed6-4188-97de-30bcc08a3f77")
            {
                OwnerId = recipe.Id,
                Value = recipeOption.Item.Id.ToString()
            };
            db.InventoryAttributes.Add(mainOutput);
            await db.SaveChangesAsync();
            return Ok(recipe);
        }
    }
}