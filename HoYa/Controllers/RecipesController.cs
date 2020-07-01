using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System.Data.Entity;
using System;
using HoYa.Models;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Web;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace HoYa.Controllers
{
    //[Authorize]
    public class RecipesController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public IQueryable<Recipe> GetRecipes(
            string anyLike = "",
            Guid? statusId = null
        )
        {
            if (anyLike == null) anyLike = "";

            return db.Recipes.Where(x =>
            (x.Status.Value == "啟用" || statusId == null) &&
            x.Value.ToString().Contains(anyLike)
            );
        }

        [Route("api/Recipes/List")]
        public async Task<IHttpActionResult> GetRecipesList(
            string anyLike = "",
              Guid? inventoryId = null,
            Guid? itemId=null
        )
        {
            if (anyLike == null) anyLike = "";
            //Guid itemId = new Guid("520934b7-82ed-457e-992f-1bb0cfd3749f");//配方
            //Guid itemId = new Guid("e4036b6c-7e88-4e56-a2f6-abda7baf7825");//轉移任務
            //Guid itemId = new Guid("a8867dc9-ae34-48e4-841b-bcbfc826f23b");//工單
            Guid ownerId = new Guid("940F7CA9-1C56-468C-A9B9-AF9EADE03872");//owner
                                                                            //Guid targetId = new Guid("428208E8-509B-4B66-BABC-1EF59D283EFF");//target

   
            HashSet<Guid?> xTargetItemIds = new HashSet<Guid?>();
            HashSet<Guid?> xTargetsItemIds = new HashSet<Guid?>();
            HashSet<Guid?> x1ItemIds = new HashSet<Guid?>();
            HashSet<Guid?> x1TargetItemIds = new HashSet<Guid?>();
            HashSet<Guid?> x2ItemIds = new HashSet<Guid?>();
            HashSet<Guid?> x1Ids = new HashSet<Guid?>();
            JObject jObject = new JObject();
            List<ItemAttribute> itemAttributes= new List<ItemAttribute>();

            if (itemId != null) {
                itemAttributes = db.ItemAttributes.Where(x => x.OwnerId == itemId).ToList();
                foreach (ItemAttribute itemAttribute in itemAttributes.Where(x => x.Target.ValueType == "存量"))
                {
                    if (itemAttribute.Target.ValueNumber == 1)
                    {
                        //存量=1的
                        foreach (string attributeItemId in itemAttribute.Target.ItemIds.Split(','))
                        {
                            if (attributeItemId.Length == 36)
                            {
                                Guid xTargetItemId = new Guid(attributeItemId);
                                xTargetItemIds.Add(xTargetItemId);
                            }
                        }
                    }
                    else
                    {
                        //存量!=1的
                        foreach (string attributeItemId in itemAttribute.Target.ItemIds.Split(','))
                        {
                            if (attributeItemId.Length == 36)
                            {
                                Guid xTargetsItemId = new Guid(attributeItemId);
                                xTargetsItemIds.Add(xTargetsItemId);
                            }
                        }
                    }
                }

                foreach (ItemAttribute itemAttribute in itemAttributes.Where(x => x.Target.ValueType == "關聯"))
                {
                    //存量!=1的
                    foreach (string attributeItemId in itemAttribute.Target.ItemIds.Split(','))
                    {
                        if (attributeItemId.Length == 36)
                        {
                            Guid x1ItemId = new Guid(attributeItemId);
                            x1ItemIds.Add(x1ItemId);
                        }
                    }
                }

                foreach (Guid x1ItemId in x1ItemIds)
                {
                    foreach (ItemAttribute itemAttribute in db.ItemAttributes.Where(x => x.OwnerId == x1ItemId && x.Target.ValueType == "存量"))
                    {
                        if (itemAttribute.Target.ValueNumber == 1)
                        {
                            //關聯=1的 //存量
                            foreach (string attributeItemId in itemAttribute.Target.ItemIds.Split(','))
                            {
                                if (attributeItemId.Length == 36)
                                {
                                    Guid x1TargetItemId = new Guid(attributeItemId);
                                    x1TargetItemIds.Add(x1TargetItemId);
                                }
                            }
                        }
                        else
                        {
                            //關聯!=1的
                            foreach (string attributeItemId in itemAttribute.Target.ItemIds.Split(','))
                            {
                                if (attributeItemId.Length == 36)
                                {
                                    Guid x2ItemId = new Guid(attributeItemId);
                                    x2ItemIds.Add(x2ItemId);
                                }
                            }
                        }
                    }
                }
            }
            IQueryable<Inventory> queriedInventories = db.Inventories;
            if(itemId!=null) queriedInventories = queriedInventories.Where(x => x.ItemId == itemId);
            if (inventoryId!=null) queriedInventories = queriedInventories.Where(x => x.Position.TargetId == inventoryId);
            if (anyLike != "")
            {
                queriedInventories = queriedInventories.Where(x => x.No.Contains(anyLike));

                if (itemAttributes.Count() > 0)
                {
                    if (itemAttributes.Count(x => x.Target.ValueType == "值") > 0)
                    {
                        queriedInventories = queriedInventories.Union(
                                                db.Inventories.Where(x =>
                                                    x.ItemId == itemId &&
                                                    db.InventoryAttributes.Any(y =>
                                                        x.Id == y.OwnerId &&
                                                        y.Value.Contains(anyLike) &&
                                                        db.ItemAttributes.Any(z =>
                                                            z.TargetId == y.TargetId &&
                                                            z.OwnerId == itemId &&
                                                            z.Target.ValueType == "值"
                                                        )
                                                    )
                                                )
                                            );

                    }

                    if (itemAttributes.Count(x => x.Target.ValueType == "品項") > 0)
                    {
                        queriedInventories = queriedInventories.Union(
                                                db.Inventories.Where(x =>
                                                    x.ItemId == itemId &&
                                                    db.InventoryAttributes.Any(y =>
                                                        x.Id == y.OwnerId &&
                                                        db.Items.Any(xTarget =>
                                                            y.Value == xTarget.Id.ToString() &&
                                                            xTarget.Code.Contains(anyLike)
                                                        )
                                                    )
                                                )
                                            );

                    } 
                    
                    if (xTargetItemIds.Count() > 0)//存量=1 attribute
                    {
                        queriedInventories = queriedInventories.Union(
                            db.Inventories.Where(x =>
                                x.ItemId == itemId &&
                                db.InventoryAttributes.Any(y =>
                                    x.Id == y.OwnerId &&
                                    db.Inventories.Any(xTarget =>
                                        y.Value == xTarget.Id.ToString() &&
                                        xTargetItemIds.Contains(xTarget.ItemId) &&
                                        xTarget.No.Contains(anyLike)
                                    )
                                )
                            )
                        );
                    }

                    if (xTargetsItemIds.Count() > 0)//存量!=1 attributes
                    {
                        queriedInventories = queriedInventories.Union(
                            db.Inventories.Where(x =>
                                x.ItemId == itemId &&
                                db.InventoryAttributes.Any(y =>
                                    x.Id.ToString() == y.Value &&
                                    y.TargetId == ownerId &&
                                    db.Inventories.Any(x1 =>
                                        x1.Id == y.OwnerId &&
                                        xTargetsItemIds.Contains(x1.ItemId) &&
                                        x1.No.Contains(anyLike)
                                    )
                                )
                            )
                        );
                    }

                    if (x1TargetItemIds.Count() > 0)//關係==1 
                    {
                        queriedInventories = queriedInventories.Union(
                            db.Inventories.Where(x =>
                                x.ItemId == itemId &&
                                db.InventoryAttributes.Any(y =>
                                    x.Id.ToString() == y.Value &&
                                    y.TargetId == ownerId &&
                                    db.Inventories.Any(x1 =>
                                        x1.Id == y.OwnerId &&
                                        x1ItemIds.Contains(x1.ItemId) &&
                                        db.InventoryAttributes.Any(y1 =>
                                            x1.Id == y1.OwnerId &&
                                            db.Inventories.Any(x1Target =>
                                                y1.Value == x1Target.Id.ToString() &&
                                                x1TargetItemIds.Contains(x1Target.ItemId) &&
                                                x1Target.No.Contains(anyLike)
                                            )
                                        )
                                    )
                                )
                            )
                        );
                    }

                    if (x2ItemIds.Count() > 0)//關係!=1
                    {
                        queriedInventories = queriedInventories.Union(
                            db.Inventories.Where(x =>
                                x.ItemId == itemId &&
                                db.InventoryAttributes.Any(y =>
                                    x.Id.ToString() == y.Value &&
                                    y.TargetId == ownerId &&
                                    db.Inventories.Any(x1 =>
                                        x1.Id == y.OwnerId &&
                                        x1ItemIds.Contains(x1.ItemId) &&
                                        db.InventoryAttributes.Any(y1 =>
                                            x1.Id.ToString() == y1.Value &&
                                            y1.TargetId == ownerId &&
                                            db.Inventories.Any(x2 =>
                                                x2.Id == y1.OwnerId &&
                                                x2ItemIds.Contains(x2.ItemId) &&
                                                x2.No.Contains(anyLike)
                                            )
                                        )
                                    )
                                )
                            )
                        );
                    }
                }
            }

            return Ok(queriedInventories.Select(x => 
                new InventoryBase
                {
                    Id = x.Id,
                    No = x.No
                }
            ).Take(400));
        }

        [Route("api/Recipes/Relationships/{recipeId}")]
        public async Task<IHttpActionResult> GetRecipesRelationships(
            string recipeId
        )
        {
            Guid itemId = new Guid("520934b7-82ed-457e-992f-1bb0cfd3749f");
            Guid ownerId = new Guid("940F7CA9-1C56-468C-A9B9-AF9EADE03872");
            JObject jObject = new JObject();

            foreach (ItemAttribute itemAttribute in db.ItemAttributes.Where(x => x.OwnerId == itemId))//recipe底下所有屬性
            {


                HashSet<Inventory> targetInventories = new HashSet<Inventory>();
                foreach (string attributeItemId in itemAttribute.Target.ItemIds.Split(','))
                {
                    Guid relationshipItemId = new Guid(attributeItemId);

                    var v = db.Inventories.FirstOrDefault(i =>
                    i.ItemId == relationshipItemId &&
                    db.InventoryAttributes.Any(j =>
                        j.TargetId == ownerId &&
                        j.OwnerId == i.Id &&
                        j.Value == recipeId
                    )
                    );

                    targetInventories.UnionWith(db.Inventories.Where(x =>
                    db.InventoryAttributes.Any(y =>
                        y.Target.Value == "target" &&
                        y.OwnerId == db.Inventories.FirstOrDefault(i =>
                                        i.ItemId == relationshipItemId &&
                                         db.InventoryAttributes.Any(j =>
                                             j.TargetId == ownerId &&
                                             j.OwnerId == i.Id &&
                                             j.Value == recipeId
                                         )
                                     ).Id &&
                        y.Value == x.Id.ToString()
                    )
                ).ToHashSet());
                }
                jObject.Add(new JProperty(itemAttribute.Target.Value, JToken.FromObject(targetInventories.Select(x => new InventoryBase { Id = x.Id, No = x.No }))));
            }


            return Ok(jObject);
        }

        [Route("api/Recipes/Detail/{id}")]
        public async Task<IHttpActionResult> GetRecipeDetail(Guid id)
        {
            RecipeDetail recipeDetail = await db.Recipes.Where(x => x.Id == id).Select(x => new RecipeDetail
            {
                Id = x.Id,
                Value = x.Value,
                Photo = x.Photo.Target.Path,
                Status = x.Status.Value
            }).FirstOrDefaultAsync();
            if (recipeDetail == null)
            {
                return NotFound();
            }

            return Ok(recipeDetail);
        }

        [Route("api/Recipes/By")]
        public async Task<IHttpActionResult> GetRecipeBy(
             Guid? definitionId = null,
            string value = "",
            string code = ""
        )
        {
            if (value == null) value = "";
            if (code == null) code = "";
            return Ok(await db.Recipes.FirstOrDefaultAsync(x =>
            (x.Value == value || value == "") &&
            (x.Code == code || code == "")
            ));
        }

        [Route("api/Recipes/Select")]
        public IQueryable<RecipeSelect> GetRecipeSelects(
             Guid? definitionId = null,
            string valueLike = "",
            string codeLike = "",
            Guid? statusId = null
        )
        {
            if (valueLike == null) valueLike = "";
            if (codeLike == null) codeLike = "";
            return db.Recipes.Where(x =>
            (x.StatusId == statusId || statusId == null) &&
            x.Value.ToString().Contains(valueLike) &&
            x.Code.ToString().Contains(codeLike)
            ).Select(option => new RecipeSelect
            {
                Id = option.Id,
                Code = option.Code,
                Value = option.Value
            }).OrderBy(x => x.Value);
        }

        [Route("api/Recipes/Count")]
        public async Task<IHttpActionResult> GetRecipesCount(
            string value = "",
            string code = "",
            Guid? statusId = null,
            Guid? excludeId = null,
            Guid? definitionId = null
        )
        {
            if (value == null) value = "";
            if (code == null) code = "";
            Guid exclude = excludeId.GetValueOrDefault();
            return Ok(db.Recipes.Where(x =>
            (x.Id != exclude || excludeId == null) &&
            (x.StatusId == statusId || statusId == null) &&
            (x.Value == value || value == "") &&
            (x.Code == code || code == "") &&
            (!(value == "" && code == "") || excludeId == null)
            ).Count());
        }

        [Route("api/Recipes/Save/{id}")]
        public async Task<IHttpActionResult> PutRecipeSave(Guid id, RecipeSave recipeSave)
        {
            Recipe existedRecipe = await db.Recipes.FindAsync(id);
            db.Entry(existedRecipe).CurrentValues.SetValues(recipeSave.Recipe);
            await db.SaveChangesAsync();
            return Ok(existedRecipe);
        }

        public async Task<IHttpActionResult> GetRecipe(Guid id)
        {
            Recipe recipe = await db.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(recipe);
        }

        public async Task<IHttpActionResult> PostRecipe(Recipe recipe)
        {
            recipe.Status = await db.Options.FindAsync(recipe.StatusId);
            Recipe existedRecipe = await db.Recipes.FirstOrDefaultAsync(x => recipe.CreatedById == x.CreatedById && x.Status.Code == "statusC");
            if (existedRecipe == null)
            {
                db.Recipes.Add(recipe);
                await db.SaveChangesAsync();
                await db.Entry(recipe).GetDatabaseValuesAsync();
                return Ok(recipe);
            }
            return Ok(existedRecipe);
        }

        public async Task<IHttpActionResult> DeleteItem(Guid id)
        {
            Recipe recipe = await db.Recipes.FindAsync(id);
            if (recipe == null) return NotFound();
            var accountName = ConfigurationManager.AppSettings["storage:account:name"];
            var accountKey = ConfigurationManager.AppSettings["storage:account:key"];
            var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer files = blobClient.GetContainerReference("files");
            foreach (FolderFile folderFile in db.FolderFiles.Where(x => x.FolderId == id).ToList())
            {
                foreach (File file in db.Files.Where(x => x.Id == folderFile.TargetId).ToList())
                {

                    CloudBlockBlob destBlob = files.GetBlockBlobReference(file.Path);
                    if (destBlob.Exists()) destBlob.Delete();

                    db.Files.Remove(file);
                }











                db.FolderFiles.Remove(folderFile);
            }
            await db.SaveChangesAsync();
            db.Recipes.Remove(recipe);
            await db.SaveChangesAsync();
            return Ok(recipe);
        }
    }
}