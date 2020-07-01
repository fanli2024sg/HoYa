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
    public class WorkOrdersController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public async Task<Inventory> FindInventory(Guid inventoryId, Guid attributeId)
        {
            InventoryAttribute inventoryAttribute = db.InventoryAttributes.FirstOrDefault(x => x.OwnerId == inventoryId && x.TargetId == attributeId);
            if (inventoryAttribute == null) return null;
            else
            {
                Guid id = Guid.Parse(inventoryAttribute.Value);
                return db.Inventories.Find(id);
            }
        }

        public async Task<Item> FindItem(Guid inventoryId, Guid attributeId)
        {
            InventoryAttribute inventoryAttribute = db.InventoryAttributes.FirstOrDefault(x => x.OwnerId == inventoryId && x.TargetId == attributeId);
            if (inventoryAttribute == null) return null;
            else
            {
                Guid id = Guid.Parse(inventoryAttribute.Value);
                return db.Items.Find(id);
            }
        }

        public async Task<Inventory> FindRecipe(Guid itemId)
        {
            Guid attributeId_mainOutput = Guid.Parse("1873a981-eed6-4188-97de-30bcc08a3f77");
            
            InventoryAttribute inventoryAttribute = db.InventoryAttributes.FirstOrDefault(x => x.TargetId == attributeId_mainOutput && x.Value==itemId.ToString());
            if (inventoryAttribute == null) return null;
            else
            {
                return db.Inventories.Find(inventoryAttribute.OwnerId);
            }
        }

        public async Task<string> SelectAttributeValueByAttributeId(Guid inventoryId, Guid attributeId)
        {
            InventoryAttribute inventoryAttribute = db.InventoryAttributes.FirstOrDefault(x => x.TargetId == attributeId && x.OwnerId == inventoryId);
            if (inventoryAttribute == null) return null;
            else
            {
                return inventoryAttribute.Value;
            }
        }

        public IQueryable<Inventory> SelectInputsByRecipeId(string recipeId)
        {
            Guid input_itemId = Guid.Parse("e44eab53-3771-4ff9-89c1-627e6787f28f");
            Guid owner_attributeId = Guid.Parse("940f7ca9-1c56-468c-a9b9-af9eade03872");
            return db.Inventories.Where(x =>
                x.ItemId == input_itemId &&
                db.InventoryAttributes.Any(y => 
                    y.OwnerId == x.Id &&
                    y.TargetId == owner_attributeId &&
                    y.Value == recipeId
                )
            );
        }

        //產生連鎖工單
        public async Task<List<Inventory>> InsertWorkOrder(Item outputItem, float outputNumber,int sort, Guid workOrderId)
        {
            List<Inventory> workOrders = new List<Inventory>();
            Guid workOrder_itemId = Guid.Parse("a8867dc9-ae34-48e4-841b-bcbfc826f23b");
            Inventory recipe = await FindRecipe(outputItem.Id);
            if (recipe != null)
            {
                Inventory workOrder = new Inventory
                {
                    Id= workOrderId,
                    ItemId = workOrder_itemId,
                    Value = 1,
                    No = "生產" + outputNumber.ToString() + "單位的【" + outputItem.Value + "】"
                };
                db.Inventories.Add(workOrder);
                db.SaveChanges();

                //配方
                Guid recipe_attributeId = Guid.Parse("a8ba7e62-8feb-4285-aca4-ed571de603e2");
                InventoryAttribute workOrderRecipe = new InventoryAttribute
                {
                    OwnerId = workOrder.Id,
                    TargetId = recipe_attributeId,
                    Value = recipe.Id.ToString()
                };
                db.InventoryAttributes.Add(workOrderRecipe);
                //產量
                Guid quantity_attributeId = Guid.Parse("28597e59-ce72-4ff4-8a79-676d3546b13e");//產量(工單)
                InventoryAttribute workOrderQuantity = new InventoryAttribute
                {
                    OwnerId = workOrder.Id,
                    TargetId = quantity_attributeId,
                    Value = outputNumber.ToString()
                };
                db.InventoryAttributes.Add(workOrderQuantity);

                //排序
                Guid sort_attributeId = Guid.Parse("e2c306dd-e5b0-46ff-9964-ad36312fb8ac");
                InventoryAttribute workOrderSort = new InventoryAttribute
                {
                    OwnerId = workOrder.Id,
                    TargetId = sort_attributeId,
                    Value = sort.ToString()
                };

                db.InventoryAttributes.Add(workOrderSort);
                db.SaveChanges();
                workOrders.Add(workOrder);
                Guid inputItem_attributeId = Guid.Parse("10879135-3f44-496c-931e-3f56af51c771");
                Guid quantity2_attributeId = Guid.Parse("788cc8d4-fa80-471e-996f-3f33827bfa05");//數量(輸入 輸出)
                var list = SelectInputsByRecipeId(recipe.Id.ToString()).ToList();
                //底下的子材料
                foreach (Inventory input in list)
                {
                    Item inputItem = await FindItem(input.Id, inputItem_attributeId);
                    float inputNumber = float.Parse(await SelectAttributeValueByAttributeId(input.Id, quantity2_attributeId));
                    List<Inventory> subOrders = await InsertWorkOrder(inputItem, inputNumber * outputNumber, sort + 1, Guid.NewGuid());
                    workOrders = workOrders.Concat(subOrders).ToList();
                }
            }
            else
            {
                //無配方 不開工單
                //需開採購單
            }
            return workOrders;
        }

        public async Task<IHttpActionResult> PostWorkOrder(InventoryWithAttributes workOrderWithAttributes)
        {
            ICollection<InventoryAttribute> inventoryAttributes = workOrderWithAttributes.Attributes;
            Guid recipeId = Guid.Parse(inventoryAttributes.FirstOrDefault(x=>x.TargetId== Guid.Parse("a8ba7e62-8feb-4285-aca4-ed571de603e2")).Value);
            Inventory recipe = db.Inventories.Find(recipeId);
            Guid mainOutput_attributeId = Guid.Parse("1873a981-eed6-4188-97de-30bcc08a3f77");//主產品
            Item outputItem = await FindItem(recipe.Id, mainOutput_attributeId);
            List<Inventory> workOrders = new List<Inventory>();
            if (outputItem != null)
            {
                Guid quantity_attributeId = Guid.Parse("28597e59-ce72-4ff4-8a79-676d3546b13e");//產量(工單)
                float outputNumber = float.Parse(inventoryAttributes.FirstOrDefault(x => x.TargetId == Guid.Parse("28597e59-ce72-4ff4-8a79-676d3546b13e")).Value);
                workOrders = await InsertWorkOrder(outputItem, outputNumber, 1, workOrderWithAttributes.Id);
            }
            float max = 0;
            foreach (Inventory workOrder in workOrders)
            {                
                Guid sort_attributeId = Guid.Parse("e2c306dd-e5b0-46ff-9964-ad36312fb8ac");
                float sort = float.Parse(await SelectAttributeValueByAttributeId(workOrder.Id, sort_attributeId));
                if (sort > max) max = sort;
            }
            max++;
            foreach (Inventory workOrder in workOrders)
            { 
                Guid sort_attributeId = Guid.Parse("e2c306dd-e5b0-46ff-9964-ad36312fb8ac");
                float newSort = max - float.Parse(await SelectAttributeValueByAttributeId(workOrder.Id, sort_attributeId));
                InventoryAttribute sort = db.InventoryAttributes.FirstOrDefault(x => x.OwnerId == workOrder.Id && x.TargetId == sort_attributeId);
                sort.Value = newSort.ToString();
                InventoryAttribute existedSort = db.InventoryAttributes.Find(sort.Id);
                db.Entry(existedSort).CurrentValues.SetValues(sort);             
            } 
            await db.SaveChangesAsync();
            return Ok(db.Inventories.Where(x => x.Id == workOrderWithAttributes.Id).Select(x => new
            {
                id = x.Id,
                no = x.No,
                value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                itemId = x.ItemId,
                itemValue = x.Item.Value,
                positionId = x.PositionId,
                positionTargetId = x.Position.TargetId,
                positionTargetNo = x.Position.Target.No,
                positionCreatedById = x.Position.CreatedById,
                positionCreatedByNo = x.Position.CreatedBy.No,
                positionPreOwnerId = x.Position.PreOwnerId,
                positionPreOwnerNo = x.Position.PreOwner.No,
                positionStartDate = x.Position.StartDate
            }).FirstOrDefault(x => x.id == workOrderWithAttributes.Id));
        }
    }
}