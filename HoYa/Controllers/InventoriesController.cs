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
    public class InventoriesController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        [Route("api/Inventories")]
        public async Task<IHttpActionResult> GetInventories(
            string anyLike = "",
            int? take = 200,
            Guid? itemId = null,
            Guid? inventoryId = null,
            Guid? categoryId = null,
            string itemIds = "",
            string inventoryIds = "",
            string categoryIds = ""
        )
        {
            if (anyLike == null) anyLike = "";
            else anyLike = HttpUtility.UrlDecode(anyLike);
            IQueryable<Inventory> inventories = null;
            IQueryable<Inventory> inventories1 = null;
            IQueryable<Inventory> inventories2 = null;
            IQueryable<Inventory> inventories3 = null;
            if (inventoryIds == null) inventoryIds = "";
            if (itemIds == null) itemIds = "";
            if (categoryIds == null) categoryIds = "";
            if (categoryIds != "")
            {
                IQueryable<string> _categoryIds = categoryIds.Split(',').AsQueryable();

                if (_categoryIds.Count() == 1)
                {
                    categoryId =Guid.Parse( _categoryIds.FirstOrDefault());
                    IQueryable<Guid?> _itemIds = db.ItemCategories.Where(x => x.TargetId == categoryId).Select(x => x.OwnerId).Distinct();
                    inventories1 = db.Inventories.Where(x => _itemIds.Contains(x.ItemId));
                }
                else
                {
                    if (_categoryIds.Count() > 1) {
                        IQueryable<Guid?> _itemIds = db.ItemCategories.Where(x => _categoryIds.Contains(x.TargetId.ToString())).Select(x => x.OwnerId).Distinct();
                        inventories1 = db.Inventories.Where(x => _itemIds.Contains(x.ItemId));
                    }
                }
            }
            else
            {
                if (categoryId != null)
                {
                    IQueryable<Guid?> _itemIds = db.ItemCategories.Where(x => x.TargetId == categoryId).Select(x => x.OwnerId).Distinct();
                    inventories1 = db.Inventories.Where(x => _itemIds.Contains(x.ItemId));
                }
            }

            if (itemIds != "")
            {
                IQueryable<string> _itemIds = itemIds.Split(',').AsQueryable();
                if (_itemIds.Count() == 1)
                {
                    itemId = Guid.Parse(_itemIds.FirstOrDefault());
                    inventories2 = db.Inventories.Where(x => x.ItemId == itemId);
                }
                else
                {
                    if(_itemIds.Count()>1) inventories2 = db.Inventories.Where(x => _itemIds.Contains(x.ItemId.ToString()));
                }
            }
            else
            {
                if (itemId != null)
                {
                    inventories2=db.Inventories.Where(x => x.ItemId == itemId);
                }
            }

            if (inventoryIds != "")
            {
                IQueryable<string> _inventoryIds = inventoryIds.Split(',').AsQueryable();
                if (_inventoryIds.Count() == 1)
                {
                    inventoryId = Guid.Parse(_inventoryIds.FirstOrDefault());
                    inventories3 = db.Inventories.Where(x => x.Position.TargetId == inventoryId);
                }
                else
                {
                    inventories3 = db.Inventories.Where(x => _inventoryIds.Contains(x.Position.TargetId.ToString()));
                }
            }
            else
            {
                if (inventoryId != null)
                {
                    inventories3 = db.Inventories.Where(x => x.Position.TargetId == inventoryId);
                }
            }

            if (inventories1 != null)
            {
                inventories = inventories1;
                if (inventories2 != null) inventories = inventories.Union(inventories2);
                if (inventories3 != null) inventories = inventories.Union(inventories3);
            }
            if (inventories2 != null)
            {
                inventories = inventories2;
                if (inventories3 != null) inventories = inventories.Union(inventories3);
            }
            if (inventories3 != null) inventories = inventories3;
            if (anyLike != "") inventories = inventories.Where(x => x.No.Contains(anyLike));

            return Ok(inventories.Take(take.GetValueOrDefault()).Select(x => new { id = x.Id, no = x.No, value = x.Value }));
        }

        [Route("api/Inventories")]
        public async Task<IHttpActionResult> PostInventory(Inventory inventory)
        {
            if (inventory.Value == null)
            {
                string userId = HttpContext.Current.User.Identity.Name;
                Guid inventoryId = (await db.Inventories.FirstOrDefaultAsync(x => x.UserId == userId)).Id;
                inventory = db.Inventories.FirstOrDefault(x => x.No.ToString() == "" && x.CreatedById == inventoryId);
                return Ok(inventory);
            }
            db.Inventories.Add(inventory);
            await db.SaveChangesAsync();
            if (inventory.Position != null)
            {
                db.Entry(inventory).CurrentValues.SetValues(inventory);
                inventory.Position.OwnerId = inventory.Id;
                Position existedPosition = await db.Positions.FindAsync(inventory.PositionId);
                db.Entry(existedPosition).CurrentValues.SetValues(inventory.Position);
                await db.SaveChangesAsync();
            }

            return Ok(db.Inventories.Where(x => x.Id == inventory.Id).Select(x => new
            {

                id = x.Id,
                no = x.No,
                value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                itemId = x.ItemId,
                itemValue = x.Item.Value,
                positionTargetId = x.Position.TargetId,
                positionTargetNo = x.Position.Target.No,
                positionCreatedById = x.Position.CreatedById,
                positionCreatedByNo = x.Position.CreatedBy.No,
                positionPreOwnerId = x.Position.PreOwnerId,
                positionPreOwnerNo = x.Position.PreOwner.No,
                positionStartDate = x.Position.StartDate
            }).FirstOrDefault(x => x.id == inventory.Id));
        }








































        [Route("api/Inventories/Details/{id}")]
        public async Task<IHttpActionResult> GetInventoryDetails(
            Guid id
        )
        {
            Guid ownerId = new Guid("940F7CA9-1C56-468C-A9B9-AF9EADE03872");//itemAttribute.target='owner'
            Guid ownerAttributeId = new Guid("C0DEAA47-CC6E-4A8C-9D43-FE5A8D14E019");//itemAttribute.target='ownerAttribute'
            Guid targetId = new Guid("428208E8-509B-4B66-BABC-1EF59D283EFF");//itemAttribute.target='target'
            Inventory inventory = await db.Inventories.FindAsync(id);
            JObject jObject = new JObject();
            foreach (ItemAttribute itemAttribute in db.ItemAttributes.Where(x => x.OwnerId == inventory.ItemId))//recipe底下所有屬性
            {
                Entities.Attribute attribute = itemAttribute.Target;
                object jToken = null;
                switch (attribute.ValueType)
                {
                    case "細節":
                        HashSet<Guid?> xTargetsItemIds = new HashSet<Guid?>();
                        foreach (string attributeItemId in attribute.ItemIds.Split(','))
                        {
                            if (attributeItemId.Length == 36)
                            {
                                Guid xTargetsItemId = new Guid(attributeItemId);
                                xTargetsItemIds.Add(xTargetsItemId);
                            }
                        }
                        jToken = JToken.FromObject(db.Inventories.Where(xTarget =>
                            xTargetsItemIds.Contains(xTarget.ItemId) &&
                            db.InventoryAttributes.Any(y =>
                                y.OwnerId == xTarget.Id &&
                                y.TargetId == ownerId &&
                                y.Value == id.ToString()
                            ) &&
                            db.InventoryAttributes.Any(y =>
                                y.OwnerId == xTarget.Id &&
                                y.TargetId == ownerAttributeId &&
                                y.Value == attribute.Id.ToString()
                            )
                        ).Select(xTarget => new { id = xTarget.Id, no = xTarget.No }));
                        break;
                    case "關聯":
                        HashSet<Guid?> x1ItemIds = new HashSet<Guid?>();
                        HashSet<Guid?> x1TargetItemIds = new HashSet<Guid?>();
                        foreach (string attributeItemId in attribute.ItemIds.Split(','))
                        {
                            if (attributeItemId.Length == 36)
                            {
                                Guid x1ItemId = new Guid(attributeItemId);
                                x1ItemIds.Add(x1ItemId);
                            }
                        }

                        foreach (Guid x1ItemId in x1ItemIds)
                        {
                            foreach (ItemAttribute itemAttribute2 in db.ItemAttributes.Where(x => x.OwnerId == x1ItemId && x.Target.ValueType == "存量"))
                            {
                                if (itemAttribute2.Target.ValueNumber == 1)
                                {
                                    //關聯=1的 //存量
                                    foreach (string attributeItemId in itemAttribute2.Target.ItemIds.Split(','))
                                    {
                                        if (attributeItemId.Length == 36)
                                        {
                                            Guid x1TargetItemId = new Guid(attributeItemId);
                                            x1TargetItemIds.Add(x1TargetItemId);
                                        }
                                    }
                                }
                            }
                        }
                        jToken = JToken.FromObject(db.Inventories.Where(x1Target =>
                            x1TargetItemIds.Contains(x1Target.ItemId) &&
                            db.InventoryAttributes.Any(y1 =>
                                y1.Value == x1Target.Id.ToString() &&
                                y1.TargetId == targetId &&
                                db.InventoryAttributes.Any(y =>
                                    y.OwnerId == y1.OwnerId &&
                                    y.TargetId == ownerId &&
                                    y.Value == id.ToString()
                                ) &&
                                db.InventoryAttributes.Any(y =>
                                    y.OwnerId == y1.OwnerId &&
                                    y.TargetId == ownerAttributeId &&
                                    y.Value == attribute.Id.ToString()
                                )
                            )
                        ).Select(x1Target => new { id = x1Target.Id, no = x1Target.No }));
                        break;
                    default:
                        break;
                }
                if (jToken != null) jObject.Add(new JProperty(itemAttribute.Target.Code, jToken));
            }
            foreach (InventoryAttribute inventoryAttribute in db.InventoryAttributes.Where(x => x.OwnerId == inventory.Id&&x.Value!=null))//recipe底下所有屬性
            {
                Entities.Attribute attribute = inventoryAttribute.Target;
                object jToken = null;
                switch (attribute.ValueType)
                {
                    case "文字":
                        jToken = JToken.FromObject(inventoryAttribute.Value);
                        break;
                    case "數值":
                        jToken = JToken.FromObject(Decimal.Parse(inventoryAttribute.Value));
                        break;
                    case "存量":
                        List<Inventory> inventories = db.Inventories.Where(xTarget =>
                            xTarget.Id.ToString() == inventoryAttribute.Value
                        ).ToList();
                        if (inventories.Count() == 1)
                        {
                            jToken = JToken.FromObject(inventories.Select(xTarget => new { id = xTarget.Id, no = xTarget.No, photo = xTarget.Photo?.Target?.Path }).FirstOrDefault());
                        }
                        break;
                    case "品項":
                        var items = db.Items.Where(item => item.Id.ToString() == inventoryAttribute.Value).ToList();
                        if (items.Count() == 1)
                        {
                            jToken = JToken.FromObject(items.Select(xTarget => new { id = xTarget.Id, value = xTarget.Value, photo = xTarget.Photo?.Target?.Path }).FirstOrDefault());
                        }
                        break;
                    case "分類":
                        break;
                    case "轉移":
                        List<Position> positions = db.Positions.Where(x =>
                            x.Id.ToString() == inventoryAttribute.Value
                        ).ToList();
                        if (positions.Count() == 1)
                        {
                            jToken = JToken.FromObject(positions.Select(x => new
                            {
                                targetId = x.TargetId,
                                targetNo = x.Target.No,
                                ownerId = x.OwnerId,
                                ownerValue = x.Owner.Value,
                                ownerItemId = x.Owner.ItemId,
                                ownerItemValue = x.Owner.Item.Value,
                                createdByNo = x.CreatedBy.No,
                                createdById = x.CreatedById,
                                startDate = x.StartDate
                            }).FirstOrDefault());
                        }
                        break;
                    default:
                        break;
                }
                if (jToken != null && !jObject.ContainsKey(inventoryAttribute.Target.Code)) jObject.Add(new JProperty(inventoryAttribute.Target.Code, jToken));
            }
            return Ok(jObject);
        }






        [Route("api/Inventories/Count")]
        public async Task<IHttpActionResult> GetInventoriesCount(
            string no = "",
            string value = "",
            Guid? statusId = null,
            Guid? excludeId = null
        )
        {
            if (no == null) no = "";
            else no = HttpUtility.UrlDecode(no);
            if (value == null) value = "";
            Guid exclude = excludeId.GetValueOrDefault();
            return Ok(db.Inventories.Where(x =>
             x.No.ToString() != "" &&
            (x.Id != exclude || excludeId == null) &&
            (x.StatusId == statusId || statusId == null) &&
            (x.No == no || no == "") &&
            (!(value == "" && no == "") || excludeId == null)
            ).Count());
        }

        [Route("api/Inventories/Count/{inventoryId}")]
        public async Task<IHttpActionResult> GetInventoryCount(
            Guid? inventoryId = null
        )
        {
            return Ok(db.Inventories.Where(x => x.No.ToString() != "" && x.Position.TargetId == inventoryId).Count());
        }

        [Route("api/Inventories/By")]
        public async Task<IHttpActionResult> GetInventoryBy(
            float? value = null,
            string no = ""
        )
        {
            if (no == null) no = "";
            else no = HttpUtility.UrlDecode(no);
            return Ok(await db.Inventories.FirstOrDefaultAsync(x =>
             x.No.ToString() != "" &&
            (x.Value == value || value == null) &&
            (x.No == no || no == "")
            ));
        }

        public async Task<IHttpActionResult> GetInventory(Guid id)
        {
            var v = db.Segmentations.Where(y => y.OwnerId == id).Sum(y => y.Quantity);
            Inventory inventory = await db.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return Ok();
            }
            return Ok(db.Inventories.Where(x => x.Id == inventory.Id).Select(x => new
            {
                id = x.Id,
                no = x.No,
                value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                originalValue = x.Value,
                photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                itemId = x.ItemId,
                itemValue = x.Item.Value,
                positionId = x.PositionId,
                positionTargetId = x.Position.TargetId,
                positionTargetNo = x.Position.Target.No,
                positionTargetPhoto = x.Position.Target.Photo != null ? x.Position.Target.Photo.Target.Path : x.Position.Target.Item.Photo.Target.Path,
                positionCreatedById = x.Position.CreatedById,
                positionCreatedByNo = x.Position.CreatedBy.No,
                positionPreOwnerId = x.Position.PreOwnerId,
                positionPreOwnerNo = x.Position.PreOwner.No,
                positionStartDate = x.Position.StartDate

            }).FirstOrDefault(x => x.id == inventory.Id));
        }

        [Route("api/Inventories/Query")]
        public async Task<IHttpActionResult> GetInventoriesQuery(
            string anyLike = "",
            string sortBy = "",
            string orderBy = "",
            int? pageIndex = 1,
            int? pageSize = 20
        )
        {
            IQueryable<Inventory> inventories = db.Inventories.Where(x => x.No.ToString() != "" && x.No.ToString().Contains(anyLike) || anyLike == null);
            switch (sortBy)
            {
                default:
                    if (orderBy == "asc") return Ok(new Query<Inventory>
                    {
                        PaginatorLength = inventories.Count(),
                        Data = inventories.OrderBy(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<Inventory>()
                    });
                    else return Ok(new Query<Inventory>
                    {
                        PaginatorLength = inventories.Count(),
                        Data = inventories.OrderByDescending(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<Inventory>()
                    });
            }
        }
       

        [Route("api/Inventories/Save")]
        public async Task<IHttpActionResult> PostInventorySave(InventorySave inventorySave)
        {
            db.Inventories.Add(inventorySave.Inventory);
            await db.SaveChangesAsync();
            if (inventorySave.Inventory.Position != null)
            {
                db.Entry(inventorySave).CurrentValues.SetValues(inventorySave);
                inventorySave.Inventory.Position.OwnerId = inventorySave.Inventory.Id;
                Position existedPosition = await db.Positions.FindAsync(inventorySave.Inventory.PositionId);
                db.Entry(existedPosition).CurrentValues.SetValues(inventorySave.Inventory.Position);
                await db.SaveChangesAsync();
            }

            foreach (var detail in inventorySave.Details)
            {

                db.InventoryAttributes.Add(detail);

            }
            await db.SaveChangesAsync();
            return Ok(inventorySave);
        }

        [Route("api/Inventories/Position")]
        public IQueryable<InventoryPosition> GetInventoryPositions(
            Guid? inventoryId = null,
            Guid? RecipeId = null,
            Guid? itemId = null,
            Guid? categoryId = null
        )
        {
            return db.Inventories.Where(inventory =>
             inventory.No.ToString() != "" &&
            inventory.Position != null &&
            (inventory.ItemId == itemId || itemId == null) &&
            ((inventory.Position.TargetId == inventoryId && inventory.Position.ArchivedDate == null) || inventoryId == null) &&
            (db.ItemCategories.Where(y => y.TargetId == categoryId && y.OwnerId == inventory.ItemId && y.EndDate == null).Count() > 0 || categoryId == null)
            ).Select(inventory => new InventoryPosition
            {
                Id = inventory.Position.TargetId,
                No = inventory.Position.Target.No
            }).Distinct();
        }


        [Route("api/Inventories/Putdown")]
        public async Task<IHttpActionResult> GetInventoryPutdowns(
            Guid inventoryId
        )
        {
            return Ok(db.Inventories.Where(x =>
                x.No.ToString() != "" &&
                x.Position.TargetId == inventoryId &&
                x.Position.ArchivedDate == null
            ).OrderBy(x => x.No).Select(x => new
            {
                id = x.Id,
                no = x.No,
                value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                itemId = x.ItemId,
                itemValue = x.Item.Value
            }));
        }

        [Route("api/Inventories/AttributeList")]
        public IQueryable<InventoryAttributeList> GetInventoryAttributeLists(
            Guid? ownerId = null
        )
        {
            return db.InventoryAttributes.Where(x => x.OwnerId == ownerId
            ).OrderBy(x => x.OwnerId).OrderBy(x => x.TargetId).Select(x => new InventoryAttributeList
            {
                Value = x.Value,
                Attribute = x.Target.Value
            });
        }

        [Route("api/Inventories/Data")]
        public async Task<IHttpActionResult> GetInventoriesData(
            string anyLike = "",
            Guid? inventoryId = null,
            Guid? itemId = null,
            string orderBy = ""
        )
        {
            Guid? orderById = db.Attributes.FirstOrDefault(x => x.Value == orderBy)?.Id;
            Guid ownerId = new Guid("940F7CA9-1C56-468C-A9B9-AF9EADE03872");//owner
            HashSet<Guid?> xTargetItemIds = new HashSet<Guid?>();
            HashSet<Guid?> xTargetsItemIds = new HashSet<Guid?>();
            HashSet<Guid?> x1ItemIds = new HashSet<Guid?>();
            HashSet<Guid?> x1TargetItemIds = new HashSet<Guid?>();
            //  HashSet<Guid?> x2ItemIds = new HashSet<Guid?>();
            HashSet<Guid?> x1Ids = new HashSet<Guid?>();
            JObject jObject = new JObject();
            List<ItemAttribute> itemAttributes = new List<ItemAttribute>();

            if (itemId != null)
            {
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
                        }/*
                        else
                        {
                            //關聯!=1的
                            foreach (string attributeItemId in itemAttribute.ItemIds.Split(','))
                            {
                                if (attributeItemId.Length == 36)
                                {
                                    Guid x2ItemId = new Guid(attributeItemId);
                                    x2ItemIds.Add(x2ItemId);
                                }
                            }
                        }*/
                    }
                }
            }
            IQueryable<Inventory> queriedInventories = db.Inventories;
            if (itemId != null) queriedInventories = queriedInventories.Where(x => x.ItemId == itemId);
            if (inventoryId != null) queriedInventories = queriedInventories.Where(x => x.Position.TargetId == inventoryId);
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
                    /*
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
                    }*/
                }
            }

            if (orderById != null)
                return Ok(
                    queriedInventories.Join(
                        db.InventoryAttributes.Where(y => y.TargetId == orderById),
                        x => x.Id,
                        y => y.OwnerId,
                        (x, y) => new
                        {
                            x.Id,
                            x.No,
                            Sort = y.Value
                        }
                    )
                );
            return Ok(
                queriedInventories.Select(
                    x => new
                    {
                        x.Id,
                        x.No
                    }
                )
            );
        }

        [Route("api/Inventories/List")]
        public async Task<IHttpActionResult> GetInventoriesList(
            string listType = "",
            string anyLike = "",
            Guid? inventoryId = null,
            Guid? itemId = null,
            string query = "",
            string orderBy = "",
            int? pageSize = 20,
            int? pageIndex = 1,
            bool descending = false
        )
        {
            if (orderBy == null) orderBy = "no";

            Guid ownerId = new Guid("940F7CA9-1C56-468C-A9B9-AF9EADE03872");//itemAttribute.target='owner'
            Guid ownerAttributeId = new Guid("C0DEAA47-CC6E-4A8C-9D43-FE5A8D14E019");//itemAttribute.target='ownerAttribute'
            Guid targetId = new Guid("428208E8-509B-4B66-BABC-1EF59D283EFF");//itemAttribute.target='target'
            HashSet<Guid?> inventoryItemIds = new HashSet<Guid?>();
            HashSet<Guid?> detailItemIds = new HashSet<Guid?>();
            HashSet<Guid?> relationshipItemIds = new HashSet<Guid?>();
            HashSet<Guid?> relationshipTargetItemIds = new HashSet<Guid?>();

            JObject jObject = new JObject();
            List<ItemAttribute> itemAttributes = new List<ItemAttribute>();
            if (anyLike == null) anyLike = "";
            if (itemId != null)
            {
                itemAttributes = db.ItemAttributes.Where(x => x.OwnerId == itemId).ToList();
                foreach (ItemAttribute itemAttribute in itemAttributes.Where(x => x.Target.ValueType == "存量"))
                {
                    foreach (string attributeItemId in itemAttribute.Target.ItemIds.Split(','))
                    {
                        if (attributeItemId.Length == 36)
                        {
                            Guid inventoryItemId = new Guid(attributeItemId);
                            inventoryItemIds.Add(inventoryItemId);
                        }
                    }
                }

                foreach (ItemAttribute itemAttribute in itemAttributes.Where(x => x.Target.ValueType == "細節"))
                {
                    foreach (string attributeItemId in itemAttribute.Target.ItemIds.Split(','))
                    {
                        if (attributeItemId.Length == 36)
                        {
                            Guid detailItemId = new Guid(attributeItemId);
                            detailItemIds.Add(detailItemId);
                        }
                    }
                }

                foreach (ItemAttribute itemAttribute in itemAttributes.Where(x => x.Target.ValueType == "關聯"))
                {
                    foreach (string attributeItemId in itemAttribute.Target.ItemIds.Split(','))
                    {
                        if (attributeItemId.Length == 36)
                        {
                            Guid relationshipItemId = new Guid(attributeItemId);
                            relationshipItemIds.Add(relationshipItemId);
                        }
                    }
                }

                foreach (Guid x1ItemId in relationshipItemIds)
                {
                    foreach (ItemAttribute itemAttribute in db.ItemAttributes.Where(x => x.OwnerId == x1ItemId && x.TargetId == targetId))
                    {
                        foreach (string attributeItemId in itemAttribute.Target.ItemIds.Split(','))
                        {
                            if (attributeItemId.Length == 36)
                            {
                                Guid relationshipTargetItemId = new Guid(attributeItemId);
                                relationshipTargetItemIds.Add(relationshipTargetItemId);
                            }
                        }
                    }
                }
            }
            IQueryable<Inventory> queriedInventories = db.Inventories;
            if (itemId != null) queriedInventories = queriedInventories.Where(x => x.ItemId == itemId);
            if (inventoryId != null) queriedInventories = queriedInventories.Where(x => x.Position.TargetId == inventoryId);




            IQueryable<Inventory> queriedInventories2 = db.Inventories;

            string[] conditions = query.Split(new string[] { "_and_" }, StringSplitOptions.None);
            foreach (string condition in conditions)
            {
                string attributeIdString = condition.Split(new string[] { "_eq_" }, StringSplitOptions.None)[0];
                if (attributeIdString.Length == 36)
                {
                    Guid attributeId = Guid.Parse(attributeIdString);
                    Entities.Attribute attribute = await db.Attributes.FindAsync(attributeId);
                    if (attribute != null)
                    {
                        string attributeValue = condition.Split(new string[] { "_eq_" }, StringSplitOptions.None)[1];
                        queriedInventories = queriedInventories.Where(x =>
                            db.InventoryAttributes.Any(y => y.OwnerId == x.Id && y.TargetId == attributeId && y.Value == attributeValue)
                        );
                        queriedInventories2 = queriedInventories2.Where(x =>
                           db.InventoryAttributes.Any(y => y.OwnerId == x.Id && y.TargetId == attributeId && y.Value == attributeValue)
                        );
                    }
                }
            }

            if (anyLike != "")
            {
                queriedInventories = queriedInventories.Where(x => x.No.Contains(anyLike));

                if (itemAttributes.Count() > 0)
                {
                    if (itemAttributes.Count(x => x.Target.ValueType == "文字") > 0)
                    {
                        queriedInventories = queriedInventories.Union(
                                                queriedInventories2.Where(x =>
                                                    x.ItemId == itemId &&
                                                    db.InventoryAttributes.Any(y =>
                                                        y.OwnerId == x.Id &&
                                                        y.Value.Contains(anyLike) &&
                                                        y.Target.ValueType == "文字"
                                                    )
                                                )
                                            );

                    }

                    if (itemAttributes.Count(x => x.Target.ValueType == "數值") > 0)
                    {
                        queriedInventories = queriedInventories.Union(
                                                queriedInventories2.Where(x =>
                                                    x.ItemId == itemId &&
                                                    db.InventoryAttributes.Any(y =>
                                                        y.OwnerId == x.Id &&
                                                        y.Value.Contains(anyLike) &&
                                                        y.Target.ValueType == "數值"
                                                    )
                                                )
                                            );
                    }

                    if (itemAttributes.Count(x => x.Target.ValueType == "品項") > 0)
                    {
                        queriedInventories = queriedInventories.Union(
                                                queriedInventories2.Where(x =>
                                                    x.ItemId == itemId &&
                                                    db.InventoryAttributes.Any(y =>
                                                        y.OwnerId == x.Id &&
                                                        db.Items.Any(item =>
                                                            item.Id.ToString() == y.Value &&
                                                            item.Code.Contains(anyLike)
                                                        ) &&
                                                        y.Target.ValueType == "品項"
                                                    )
                                                )
                                            );
                    }

                    if (inventoryItemIds.Count() > 0)//存量
                    {
                        queriedInventories = queriedInventories.Union(
                            queriedInventories2.Where(x =>
                                x.ItemId == itemId &&
                                db.InventoryAttributes.Any(y =>
                                    y.OwnerId == x.Id &&
                                    db.Inventories.Any(inventory =>
                                        inventory.Id.ToString() == y.Value &&
                                        inventoryItemIds.Contains(inventory.ItemId) &&//為了縮限範圍
                                        inventory.No.Contains(anyLike)
                                    ) &&
                                    y.Target.ValueType == "存量"
                                )
                            )
                        );
                    }

                    if (detailItemIds.Count() > 0)//細節
                    {
                        queriedInventories = queriedInventories.Union(
                            queriedInventories2.Where(x =>
                                x.ItemId == itemId &&
                                db.InventoryAttributes.Any(y => //細節中有屬性owner的值是結果
                                    y.Value == x.Id.ToString() &&
                                    y.TargetId == ownerId &&
                                    db.Inventories.Any(x1 => //細節的No包含anyLike的
                                        x1.Id == y.OwnerId &&
                                        detailItemIds.Contains(x1.ItemId) &&
                                        x1.No.Contains(anyLike)
                                    ) &&
                                    y.Target.ValueType == "存量"
                                )
                            )
                        );
                    }

                    if (relationshipTargetItemIds.Count() > 0)//關聯
                    {
                        queriedInventories = queriedInventories.Union(
                            queriedInventories2.Where(x =>
                                x.ItemId == itemId &&
                                db.InventoryAttributes.Any(y =>
                                    y.Value == x.Id.ToString() &&
                                    y.TargetId == ownerId &&
                                    db.Inventories.Any(relationship =>
                                        relationship.Id == y.OwnerId &&
                                        relationshipItemIds.Contains(relationship.ItemId) &&
                                        db.InventoryAttributes.Any(y1 =>
                                            y1.OwnerId == relationship.Id &&
                                            y1.TargetId == targetId &&
                                            db.Inventories.Any(relationshipTarget =>
                                                relationshipTarget.Id.ToString() == y1.Value &&
                                                relationshipTargetItemIds.Contains(relationshipTarget.ItemId) &&
                                                relationshipTarget.No.Contains(anyLike)
                                            ) &&
                                            y1.Target.ValueType == "存量"
                                        )
                                    ) &&
                                    y.Target.ValueType == "存量"
                                )
                            )
                        );
                    }
                }
            }

            jObject.Add(new JProperty("total", queriedInventories.Count()));
            if (listType == "general")
            {
                switch (orderBy)
                {
                    case "no":
                        if (descending) jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                        {
                            id = x.Id,
                            no = x.No,
                            value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                            photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                            itemId = x.ItemId,
                            itemValue = x.Item.Value,
                            positionTargetId = x.Position.TargetId,
                            positionTargetNo = x.Position.Target.No,
                            positionCreatedById = x.Position.CreatedById,
                            positionCreatedByNo = x.Position.CreatedBy.No,
                            positionPreOwnerId = x.Position.PreOwnerId,
                            positionPreOwnerNo = x.Position.PreOwner.No,
                            positionStartDate = x.Position.StartDate
                        }).OrderByDescending(x => x.no).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                        else jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                        {
                            id = x.Id,
                            no = x.No,
                            value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                            photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                            itemId = x.ItemId,
                            itemValue = x.Item.Value,
                            positionTargetId = x.Position.TargetId,
                            positionTargetNo = x.Position.Target.No,
                            positionCreatedById = x.Position.CreatedById,
                            positionCreatedByNo = x.Position.CreatedBy.No,
                            positionPreOwnerId = x.Position.PreOwnerId,
                            positionPreOwnerNo = x.Position.PreOwner.No,
                            positionStartDate = x.Position.StartDate
                        }).OrderBy(x => x.no).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                        break;
                    case "value":
                        if (descending) jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                        {
                            id = x.Id,
                            no = x.No,
                            value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                            photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                            itemId = x.ItemId,
                            itemValue = x.Item.Value,
                            positionTargetId = x.Position.TargetId,
                            positionTargetNo = x.Position.Target.No,
                            positionCreatedById = x.Position.CreatedById,
                            positionCreatedByNo = x.Position.CreatedBy.No,
                            positionPreOwnerId = x.Position.PreOwnerId,
                            positionPreOwnerNo = x.Position.PreOwner.No,
                            positionStartDate = x.Position.StartDate
                        }).AsEnumerable().OrderByDescending(x => Convert.ToDecimal(x.value)).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                        else jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                        {
                            id = x.Id,
                            no = x.No,
                            value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                            photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                            itemId = x.ItemId,
                            item = x.Item.Value,
                            positionTargetId = x.Position.TargetId,
                            positionTargetNo = x.Position.Target.No,
                            positionCreatedById = x.Position.CreatedById,
                            positionCreatedByNo = x.Position.CreatedBy.No,
                            positionPreOwnerId = x.Position.PreOwnerId,
                            positionPreOwnerNo = x.Position.PreOwner.No,
                            positionStartDate = x.Position.StartDate
                        }).AsEnumerable().OrderBy(x => Convert.ToDecimal(x.value)).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                        break;
                    case "itemValue":
                        if (descending) jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                        {
                            id = x.Id,
                            no = x.No,
                            value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                            photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                            itemId = x.ItemId,
                            itemValue = x.Item.Value,
                            positionTargetId = x.Position.TargetId,
                            positionTargetNo = x.Position.Target.No,
                            positionCreatedById = x.Position.CreatedById,
                            positionCreatedByNo = x.Position.CreatedBy.No,
                            positionPreOwnerId = x.Position.PreOwnerId,
                            positionPreOwnerNo = x.Position.PreOwner.No,
                            positionStartDate = x.Position.StartDate
                        }).OrderByDescending(x => x.itemValue).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                        else jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                        {
                            id = x.Id,
                            no = x.No,
                            value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                            photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                            itemId = x.ItemId,
                            itemValue = x.Item.Value,
                            positionTargetId = x.Position.TargetId,
                            positionTargetNo = x.Position.Target.No,
                            positionCreatedById = x.Position.CreatedById,
                            positionCreatedByNo = x.Position.CreatedBy.No,
                            positionPreOwnerId = x.Position.PreOwnerId,
                            positionPreOwnerNo = x.Position.PreOwner.No,
                            positionStartDate = x.Position.StartDate
                        }).OrderBy(x => x.itemValue).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                        break;
                    case "positionNo":
                        if (descending) jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                        {
                            id = x.Id,
                            no = x.No,
                            value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                            photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                            itemId = x.ItemId,
                            itemValue = x.Item.Value,
                            positionTargetId = x.Position.TargetId,
                            positionTargetNo = x.Position.Target.No,
                            positionCreatedById = x.Position.CreatedById,
                            positionCreatedByNo = x.Position.CreatedBy.No,
                            positionPreOwnerId = x.Position.PreOwnerId,
                            positionPreOwnerNo = x.Position.PreOwner.No,
                            positionStartDate = x.Position.StartDate
                        }).OrderByDescending(x => x.positionTargetNo).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                        else jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                        {
                            id = x.Id,
                            no = x.No,
                            value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                            photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                            itemId = x.ItemId,
                            itemValue = x.Item.Value,
                            positionTargetId = x.Position.TargetId,
                            positionTargetNo = x.Position.Target.No,
                            positionCreatedById = x.Position.CreatedById,
                            positionCreatedByNo = x.Position.CreatedBy.No,
                            positionPreOwnerId = x.Position.PreOwnerId,
                            positionPreOwnerNo = x.Position.PreOwner.No,
                            positionStartDate = x.Position.StartDate
                        }).OrderBy(x => x.positionTargetNo).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                        break;
                    case "positionCreatedByNo":
                        if (descending) jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                        {
                            id = x.Id,
                            no = x.No,
                            value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                            photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                            itemId = x.ItemId,
                            itemValue = x.Item.Value,
                            positionTargetId = x.Position.TargetId,
                            positionTargetNo = x.Position.Target.No,
                            positionCreatedById = x.Position.CreatedById,
                            positionCreatedByNo = x.Position.CreatedBy.No,
                            positionPreOwnerId = x.Position.PreOwnerId,
                            positionPreOwnerNo = x.Position.PreOwner.No,
                            positionStartDate = x.Position.StartDate
                        }).OrderByDescending(x => x.positionCreatedByNo).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                        else jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                        {
                            id = x.Id,
                            no = x.No,
                            value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                            photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                            itemId = x.ItemId,
                            itemValue = x.Item.Value,
                            positionTargetId = x.Position.TargetId,
                            positionTargetNo = x.Position.Target.No,
                            positionCreatedById = x.Position.CreatedById,
                            positionCreatedByNo = x.Position.CreatedBy.No,
                            positionPreOwnerId = x.Position.PreOwnerId,
                            positionPreOwnerNo = x.Position.PreOwner.No,
                            positionStartDate = x.Position.StartDate
                        }).OrderBy(x => x.positionCreatedByNo).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                        break;
                    case "positionPreOwnerNo":
                        if (descending) jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                        {
                            id = x.Id,
                            no = x.No,
                            value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                            photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                            itemId = x.ItemId,
                            itemValue = x.Item.Value,
                            positionTargetId = x.Position.TargetId,
                            positionTargetNo = x.Position.Target.No,
                            positionCreatedById = x.Position.CreatedById,
                            positionCreatedByNo = x.Position.CreatedBy.No,
                            positionPreOwnerId = x.Position.PreOwnerId,
                            positionPreOwnerNo = x.Position.PreOwner.No,
                            positionStartDate = x.Position.StartDate
                        }).OrderByDescending(x => x.positionPreOwnerNo).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                        else jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                        {
                            id = x.Id,
                            no = x.No,
                            value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                            photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                            itemId = x.ItemId,
                            itemValue = x.Item.Value,
                            positionTargetId = x.Position.TargetId,
                            positionTargetNo = x.Position.Target.No,
                            positionCreatedById = x.Position.CreatedById,
                            positionCreatedByNo = x.Position.CreatedBy.No,
                            positionPreOwnerId = x.Position.PreOwnerId,
                            positionPreOwnerNo = x.Position.PreOwner.No,
                            positionStartDate = x.Position.StartDate
                        }).OrderBy(x => x.positionPreOwnerNo).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                        break;
                    case "positionStartDate":
                        if (descending) jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                        {
                            id = x.Id,
                            no = x.No,
                            value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                            photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                            itemId = x.ItemId,
                            itemValue = x.Item.Value,
                            positionTargetId = x.Position.TargetId,
                            positionTargetNo = x.Position.Target.No,
                            positionCreatedById = x.Position.CreatedById,
                            positionCreatedByNo = x.Position.CreatedBy.No,
                            positionPreOwnerId = x.Position.PreOwnerId,
                            positionPreOwnerNo = x.Position.PreOwner.No,
                            positionStartDate = x.Position.StartDate
                        }).OrderByDescending(x => x.positionStartDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                        else jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                        {
                            id = x.Id,
                            no = x.No,
                            value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                            photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                            itemId = x.ItemId,
                            itemValue = x.Item.Value,
                            positionTargetId = x.Position.TargetId,
                            positionTargetNo = x.Position.Target.No,
                            positionCreatedById = x.Position.CreatedById,
                            positionCreatedByNo = x.Position.CreatedBy.No,
                            positionPreOwnerId = x.Position.PreOwnerId,
                            positionPreOwnerNo = x.Position.PreOwner.No,
                            positionStartDate = x.Position.StartDate
                        }).OrderBy(x => x.positionStartDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                        break;
                    default:
                        if (itemAttributes.Count() > 0)
                        {
                            Entities.Attribute attribute = await db.Attributes.FirstOrDefaultAsync(x => x.Code == orderBy);
                            if (attribute != null)
                            {
                                Guid orderById = db.Attributes.FirstOrDefault(x => x.Code == orderBy).Id;
                                switch (attribute.ValueType)
                                {
                                    case "文字":
                                        if (descending) jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                        {
                                            Sort = db.InventoryAttributes.FirstOrDefault(z => z.TargetId == orderById && z.OwnerId == x.Id).Value,
                                            Obj = new
                                            {
                                                id = x.Id,
                                                no = x.No,
                                                value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                                                photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                                                itemId = x.ItemId,
                                                itemValue = x.Item.Value,
                                                positionTargetId = x.Position.TargetId,
                                                positionTargetNo = x.Position.Target.No,
                                                positionCreatedById = x.Position.CreatedById,
                                                positionCreatedByNo = x.Position.CreatedBy.No,
                                                positionPreOwnerId = x.Position.PreOwnerId,
                                                positionPreOwnerNo = x.Position.PreOwner.No,
                                                positionStartDate = x.Position.StartDate
                                            }
                                        }).OrderByDescending(x => x.Sort).Select(x => x.Obj).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                                        else jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                        {
                                            Sort = db.InventoryAttributes.FirstOrDefault(z => z.TargetId == orderById && z.OwnerId == x.Id).Value,
                                            Obj = new
                                            {
                                                id = x.Id,
                                                no = x.No,
                                                value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                                                photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                                                itemId = x.ItemId,
                                                itemValue = x.Item.Value,
                                                positionTargetId = x.Position.TargetId,
                                                positionTargetNo = x.Position.Target.No,
                                                positionCreatedById = x.Position.CreatedById,
                                                positionCreatedByNo = x.Position.CreatedBy.No,
                                                positionPreOwnerId = x.Position.PreOwnerId,
                                                positionPreOwnerNo = x.Position.PreOwner.No,
                                                positionStartDate = x.Position.StartDate
                                            }
                                        }).OrderBy(x => x.Sort).Select(x => x.Obj).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                                        break;
                                    case "數值":
                                        if (descending) jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                        {
                                            Sort = db.InventoryAttributes.FirstOrDefault(z => z.TargetId == orderById && z.OwnerId == x.Id).Value,
                                            Obj = new
                                            {
                                                id = x.Id,
                                                no = x.No,
                                                value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                                                photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                                                itemId = x.ItemId,
                                                itemValue = x.Item.Value,
                                                positionTargetId = x.Position.TargetId,
                                                positionTargetNo = x.Position.Target.No,
                                                positionCreatedById = x.Position.CreatedById,
                                                positionCreatedByNo = x.Position.CreatedBy.No,
                                                positionPreOwnerId = x.Position.PreOwnerId,
                                                positionPreOwnerNo = x.Position.PreOwner.No,
                                                positionStartDate = x.Position.StartDate
                                            }
                                        }).AsEnumerable().OrderByDescending(x => Convert.ToDecimal(x.Sort)).Select(x => x.Obj).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                                        else jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                        {
                                            Sort = db.InventoryAttributes.FirstOrDefault(z => z.TargetId == orderById && z.OwnerId == x.Id).Value,
                                            Obj = new
                                            {
                                                id = x.Id,
                                                no = x.No,
                                                value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                                                photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                                                itemId = x.ItemId,
                                                itemValue = x.Item.Value,
                                                positionTargetId = x.Position.TargetId,
                                                positionTargetNo = x.Position.Target.No,
                                                positionCreatedById = x.Position.CreatedById,
                                                positionCreatedByNo = x.Position.CreatedBy.No,
                                                positionPreOwnerId = x.Position.PreOwnerId,
                                                positionPreOwnerNo = x.Position.PreOwner.No,
                                                positionStartDate = x.Position.StartDate
                                            }
                                        }).AsEnumerable().OrderBy(x => Convert.ToDecimal(x.Sort)).Select(x => x.Obj).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                                        break;
                                    case "存量":
                                        if (descending) jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                        {
                                            Sort = db.Inventories.FirstOrDefault(inventory =>
                                                inventoryItemIds.Contains(inventory.ItemId) &&
                                                inventory.Id.ToString() == db.InventoryAttributes.FirstOrDefault(z =>
                                                    z.TargetId == orderById &&
                                                    z.OwnerId == x.Id &&
                                                    z.Target.ValueType == "存量"
                                                ).Value
                                            ).No,
                                            Obj = new
                                            {
                                                id = x.Id,
                                                no = x.No,
                                                value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                                                photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                                                itemId = x.ItemId,
                                                itemValue = x.Item.Value,
                                                positionTargetId = x.Position.TargetId,
                                                positionTargetNo = x.Position.Target.No,
                                                positionCreatedById = x.Position.CreatedById,
                                                positionCreatedByNo = x.Position.CreatedBy.No,
                                                positionPreOwnerId = x.Position.PreOwnerId,
                                                positionPreOwnerNo = x.Position.PreOwner.No,
                                                positionStartDate = x.Position.StartDate
                                            }
                                        }).OrderByDescending(x => x.Sort).Select(x => x.Obj).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                                        else jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                        {
                                            Sort = db.Inventories.FirstOrDefault(y =>
                                                inventoryItemIds.Contains(y.ItemId) &&
                                                y.Id.ToString() == db.InventoryAttributes.FirstOrDefault(z =>
                                                    z.TargetId == orderById &&
                                                    z.OwnerId == x.Id &&
                                                    z.Target.ValueType == "存量"
                                                ).Value
                                            ).No,
                                            Obj = new
                                            {
                                                id = x.Id,
                                                no = x.No,
                                                value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                                                photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                                                itemId = x.ItemId,
                                                itemValue = x.Item.Value,
                                                positionTargetId = x.Position.TargetId,
                                                positionTargetNo = x.Position.Target.No,
                                                positionCreatedById = x.Position.CreatedById,
                                                positionCreatedByNo = x.Position.CreatedBy.No,
                                                positionPreOwnerId = x.Position.PreOwnerId,
                                                positionPreOwnerNo = x.Position.PreOwner.No,
                                                positionStartDate = x.Position.StartDate
                                            }
                                        }).OrderBy(x => x.Sort).Select(x => x.Obj).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                                        break;
                                    case "細節":
                                        if (descending) jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                        {
                                            Sort = db.InventoryAttributes.Where(y =>
                                                detailItemIds.Contains(y.Owner.ItemId) &&
                                                y.TargetId == ownerId &&
                                                y.Value == x.Id.ToString() &&
                                                db.InventoryAttributes.Any(oa =>
                                                    oa.TargetId == ownerAttributeId &&
                                                    oa.OwnerId == y.OwnerId &&
                                                    oa.Value == orderById.ToString() &&
                                                    oa.Target.ValueType == "屬性"
                                                ) &&
                                                y.Target.ValueType == "存量"
                                            ).Select(z => z.Owner.No).OrderByDescending(z => z).FirstOrDefault(),
                                            Obj = new
                                            {
                                                id = x.Id,
                                                no = x.No,
                                                value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                                                photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                                                itemId = x.ItemId,
                                                itemValue = x.Item.Value,
                                                positionTargetId = x.Position.TargetId,
                                                positionTargetNo = x.Position.Target.No,
                                                positionCreatedById = x.Position.CreatedById,
                                                positionCreatedByNo = x.Position.CreatedBy.No,
                                                positionPreOwnerId = x.Position.PreOwnerId,
                                                positionPreOwnerNo = x.Position.PreOwner.No,
                                                positionStartDate = x.Position.StartDate
                                            }
                                        }).OrderByDescending(x => x.Sort).Select(x => x.Obj).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                                        else jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                        {
                                            Sort = db.InventoryAttributes.Where(y =>
                                                 detailItemIds.Contains(y.Owner.ItemId) &&
                                                y.TargetId == ownerId &&
                                                y.Value == x.Id.ToString() &&
                                                db.InventoryAttributes.Any(oa =>
                                                    oa.TargetId == ownerAttributeId &&
                                                    oa.OwnerId == y.OwnerId &&
                                                    oa.Value == orderById.ToString() &&
                                                    oa.Target.ValueType == "屬性"
                                                ) &&
                                                y.Target.ValueType == "存量"
                                            ).Select(z => z.Owner.No).OrderBy(z => z).FirstOrDefault(),
                                            Obj = new
                                            {
                                                id = x.Id,
                                                no = x.No,
                                                value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                                                photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                                                itemId = x.ItemId,
                                                itemValue = x.Item.Value,
                                                positionTargetId = x.Position.TargetId,
                                                positionTargetNo = x.Position.Target.No,
                                                positionCreatedById = x.Position.CreatedById,
                                                positionCreatedByNo = x.Position.CreatedBy.No,
                                                positionPreOwnerId = x.Position.PreOwnerId,
                                                positionPreOwnerNo = x.Position.PreOwner.No,
                                                positionStartDate = x.Position.StartDate
                                            }
                                        }).OrderBy(x => x.Sort).Select(x => x.Obj).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));

                                        break;
                                    case "關聯":
                                        if (descending) jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                        {
                                            Sort = db.InventoryAttributes.Where(y =>
                                                y.TargetId == targetId &&
                                                db.InventoryAttributes.Any(z =>
                                                    z.TargetId == ownerId &&
                                                    z.Value == x.Id.ToString() &&
                                                    z.OwnerId == y.OwnerId &&
                                                    db.InventoryAttributes.Any(oa =>
                                                        oa.TargetId == ownerAttributeId &&
                                                        oa.OwnerId == z.OwnerId &&
                                                        oa.Value == orderById.ToString() &&
                                                        oa.Target.ValueType == "屬性"
                                                    ) &&
                                                    z.Target.ValueType == "存量"
                                                ) &&
                                                y.Target.ValueType == "存量"
                                            ).Select(y => y.Owner.No).OrderByDescending(y => y).FirstOrDefault(),
                                            Obj = new
                                            {
                                                id = x.Id,
                                                no = x.No,
                                                value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                                                photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                                                itemId = x.ItemId,
                                                itemValue = x.Item.Value,
                                                positionTargetId = x.Position.TargetId,
                                                positionTargetNo = x.Position.Target.No,
                                                positionCreatedById = x.Position.CreatedById,
                                                positionCreatedByNo = x.Position.CreatedBy.No,
                                                positionPreOwnerId = x.Position.PreOwnerId,
                                                positionPreOwnerNo = x.Position.PreOwner.No,
                                                positionStartDate = x.Position.StartDate
                                            }
                                        }).OrderByDescending(x => x.Sort).Select(x => x.Obj).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                                        else jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                        {
                                            Sort = db.InventoryAttributes.Where(y =>
                                                y.TargetId == targetId &&
                                                db.InventoryAttributes.Any(z =>
                                                    z.TargetId == ownerId &&
                                                    z.Value == x.Id.ToString() &&
                                                    z.OwnerId == y.OwnerId &&
                                                    db.InventoryAttributes.Any(oa =>
                                                        oa.TargetId == ownerAttributeId &&
                                                        oa.OwnerId == z.OwnerId &&
                                                        oa.Value == orderById.ToString() &&
                                                        oa.Target.ValueType == "屬性"
                                                    ) &&
                                                    z.Target.ValueType == "存量"
                                                ) &&
                                                y.Target.ValueType == "存量"
                                            ).Select(y => y.Owner.No).OrderBy(y => y).FirstOrDefault(),
                                            Obj = new
                                            {
                                                id = x.Id,
                                                no = x.No,
                                                value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                                                photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                                                itemId = x.ItemId,
                                                itemValue = x.Item.Value,
                                                positionTargetId = x.Position.TargetId,
                                                positionTargetNo = x.Position.Target.No,
                                                positionCreatedById = x.Position.CreatedById,
                                                positionCreatedByNo = x.Position.CreatedBy.No,
                                                positionPreOwnerId = x.Position.PreOwnerId,
                                                positionPreOwnerNo = x.Position.PreOwner.No,
                                                positionStartDate = x.Position.StartDate
                                            }
                                        }).OrderBy(x => x.Sort).Select(x => x.Obj).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                                        break;
                                    default:
                                        jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                        {
                                            id = x.Id,
                                            no = x.No,
                                            value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                                            photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                                            itemId = x.ItemId,
                                            itemValue = x.Item.Value,
                                            positionTargetId = x.Position.TargetId,
                                            positionTargetNo = x.Position.Target.No,
                                            positionCreatedById = x.Position.CreatedById,
                                            positionCreatedByNo = x.Position.CreatedBy.No,
                                            positionPreOwnerId = x.Position.PreOwnerId,
                                            positionPreOwnerNo = x.Position.PreOwner.No,
                                            positionStartDate = x.Position.StartDate
                                        }).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                                        break;
                                }
                            }
                            else
                            {
                                if (descending) jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                {
                                    id = x.Id,
                                    no = x.No,
                                    value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                                    photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                                    itemId = x.ItemId,
                                    itemValue = x.Item.Value,
                                    positionTargetId = x.Position.TargetId,
                                    positionTargetNo = x.Position.Target.No,
                                    positionCreatedById = x.Position.CreatedById,
                                    positionCreatedByNo = x.Position.CreatedBy.No,
                                    positionPreOwnerId = x.Position.PreOwnerId,
                                    positionPreOwnerNo = x.Position.PreOwner.No,
                                    positionStartDate = x.Position.StartDate
                                }).OrderByDescending(x => x.no).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                                else jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                {
                                    id = x.Id,
                                    no = x.No,
                                    value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                                    photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                                    itemId = x.ItemId,
                                    itemValue = x.Item.Value,
                                    positionTargetId = x.Position.TargetId,
                                    positionTargetNo = x.Position.Target.No,
                                    positionCreatedById = x.Position.CreatedById,
                                    positionCreatedByNo = x.Position.CreatedBy.No,
                                    positionPreOwnerId = x.Position.PreOwnerId,
                                    positionPreOwnerNo = x.Position.PreOwner.No,
                                    positionStartDate = x.Position.StartDate
                                }).OrderBy(x => x.no).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                            }
                        }
                        else
                        {
                            if (descending) jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                            {
                                id = x.Id,
                                no = x.No,
                                value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                                photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                                itemId = x.ItemId,
                                itemValue = x.Item.Value,
                                positionTargetId = x.Position.TargetId,
                                positionTargetNo = x.Position.Target.No,
                                positionCreatedById = x.Position.CreatedById,
                                positionCreatedByNo = x.Position.CreatedBy.No,
                                positionPreOwnerId = x.Position.PreOwnerId,
                                positionPreOwnerNo = x.Position.PreOwner.No,
                                positionStartDate = x.Position.StartDate
                            }).OrderByDescending(x => x.no).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                            else jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                            {
                                id = x.Id,
                                no = x.No,
                                value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                                photo = x.Photo != null ? x.Photo.Target.Path : x.Item.Photo.Target.Path,
                                itemId = x.ItemId,
                                itemValue = x.Item.Value,
                                positionTargetId = x.Position.TargetId,
                                positionTargetNo = x.Position.Target.No,
                                positionCreatedById = x.Position.CreatedById,
                                positionCreatedByNo = x.Position.CreatedBy.No,
                                positionPreOwnerId = x.Position.PreOwnerId,
                                positionPreOwnerNo = x.Position.PreOwner.No,
                                positionStartDate = x.Position.StartDate
                            }).OrderBy(x => x.no).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                        }
                        break;
                }
            }
            else
            {
                switch (orderBy)
                {
                    case "no":
                        if (descending) jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                        {
                            id = x.Id,
                            no = x.No,
                            createdDate = x.CreatedDate,
                            createdByNo = x.CreatedBy.No
                        }).OrderByDescending(x => x.no).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                        else jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                        {
                            id = x.Id,
                            no = x.No,
                            createdDate = x.CreatedDate,
                            createdByNo = x.CreatedBy.No
                        }).OrderBy(x => x.no).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                        break;
                    default:
                        if (itemAttributes.Count() > 0)
                        {
                            Entities.Attribute attribute = await db.Attributes.FirstOrDefaultAsync(x => x.Code == orderBy);
                            if (attribute != null)
                            {
                                Guid orderById = db.Attributes.FirstOrDefault(x => x.Code == orderBy).Id;
                                switch (attribute.ValueType)
                                {
                                    case "文字":
                                        if (descending) jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                        {
                                            Sort = db.InventoryAttributes.FirstOrDefault(z => z.TargetId == orderById && z.OwnerId == x.Id).Value,
                                            Obj = new
                                            {
                                                id = x.Id,
                                                no = x.No,
                                                createdDate = x.CreatedDate,
                                                createdByNo = x.CreatedBy.No
                                            }
                                        }).OrderByDescending(x => x.Sort).Select(x => x.Obj).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                                        else jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                        {
                                            Sort = db.InventoryAttributes.FirstOrDefault(z => z.TargetId == orderById && z.OwnerId == x.Id).Value,
                                            Obj = new
                                            {
                                                id = x.Id,
                                                no = x.No,
                                                createdDate = x.CreatedDate,
                                                createdByNo = x.CreatedBy.No
                                            }
                                        }).OrderBy(x => x.Sort).Select(x => x.Obj).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                                        break;
                                    case "數值":
                                        if (descending) jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                        {
                                            Sort = db.InventoryAttributes.FirstOrDefault(z => z.TargetId == orderById && z.OwnerId == x.Id).Value,
                                            Obj = new
                                            {
                                                id = x.Id,
                                                no = x.No,
                                                createdDate = x.CreatedDate,
                                                createdByNo = x.CreatedBy.No
                                            }
                                        }).AsEnumerable().OrderByDescending(x => Convert.ToDecimal(x.Sort)).Select(x => x.Obj).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                                        else jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                        {
                                            Sort = db.InventoryAttributes.FirstOrDefault(z => z.TargetId == orderById && z.OwnerId == x.Id).Value,
                                            Obj = new
                                            {
                                                id = x.Id,
                                                no = x.No,
                                                createdDate = x.CreatedDate,
                                                createdByNo = x.CreatedBy.No
                                            }
                                        }).AsEnumerable().OrderBy(x => Convert.ToDecimal(x.Sort)).Select(x => x.Obj).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                                        break;
                                    case "存量":
                                        if (descending) jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                        {
                                            Sort = db.Inventories.FirstOrDefault(inventory =>
                                                inventoryItemIds.Contains(inventory.ItemId) &&
                                                inventory.Id.ToString() == db.InventoryAttributes.FirstOrDefault(z =>
                                                    z.TargetId == orderById &&
                                                    z.OwnerId == x.Id &&
                                                    z.Target.ValueType == "存量"
                                                ).Value
                                            ).No,
                                            Obj = new
                                            {
                                                id = x.Id,
                                                no = x.No,
                                                createdDate = x.CreatedDate,
                                                createdByNo = x.CreatedBy.No
                                            }
                                        }).OrderByDescending(x => x.Sort).Select(x => x.Obj).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                                        else jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                        {
                                            Sort = db.Inventories.FirstOrDefault(y =>
                                                inventoryItemIds.Contains(y.ItemId) &&
                                                y.Id.ToString() == db.InventoryAttributes.FirstOrDefault(z =>
                                                    z.TargetId == orderById &&
                                                    z.OwnerId == x.Id &&
                                                    z.Target.ValueType == "存量"
                                                ).Value
                                            ).No,
                                            Obj = new
                                            {
                                                id = x.Id,
                                                no = x.No,
                                                createdDate = x.CreatedDate,
                                                createdByNo = x.CreatedBy.No
                                            }
                                        }).OrderBy(x => x.Sort).Select(x => x.Obj).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                                        break;
                                    case "細節":
                                        if (descending) jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                        {
                                            Sort = db.InventoryAttributes.Where(y =>
                                                detailItemIds.Contains(y.Owner.ItemId) &&
                                                y.TargetId == ownerId &&
                                                y.Value == x.Id.ToString() &&
                                                db.InventoryAttributes.Any(oa =>
                                                    oa.TargetId == ownerAttributeId &&
                                                    oa.OwnerId == y.OwnerId &&
                                                    oa.Value == orderById.ToString() &&
                                                    oa.Target.ValueType == "屬性"
                                                ) &&
                                                y.Target.ValueType == "存量"
                                            ).Select(z => z.Owner.No).OrderByDescending(z => z).FirstOrDefault(),
                                            Obj = new
                                            {
                                                id = x.Id,
                                                no = x.No,
                                                createdDate = x.CreatedDate,
                                                createdByNo = x.CreatedBy.No
                                            }
                                        }).OrderByDescending(x => x.Sort).Select(x => x.Obj).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                                        else jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                        {
                                            Sort = db.InventoryAttributes.Where(y =>
                                                detailItemIds.Contains(y.Owner.ItemId) &&
                                                y.TargetId == ownerId &&
                                                y.Value == x.Id.ToString() &&
                                                db.InventoryAttributes.Any(oa =>
                                                    oa.TargetId == ownerAttributeId &&
                                                    oa.OwnerId == y.OwnerId &&
                                                    oa.Value == orderById.ToString() &&
                                                    oa.Target.ValueType == "屬性"
                                                ) &&
                                                y.Target.ValueType == "存量"
                                            ).Select(z => z.Owner.No).OrderBy(z => z).FirstOrDefault(),
                                            Obj = new
                                            {
                                                id = x.Id,
                                                no = x.No,
                                                createdDate = x.CreatedDate,
                                                createdByNo = x.CreatedBy.No
                                            }
                                        }).OrderBy(x => x.Sort).Select(x => x.Obj).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));

                                        break;
                                    case "關聯":
                                        if (descending) jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                        {
                                            Sort = db.InventoryAttributes.Where(y =>
                                                y.TargetId == targetId &&
                                                db.InventoryAttributes.Any(z =>
                                                    z.TargetId == ownerId &&
                                                    z.Value == x.Id.ToString() &&
                                                    z.OwnerId == y.OwnerId &&
                                                    db.InventoryAttributes.Any(oa =>
                                                        oa.TargetId == ownerAttributeId &&
                                                        oa.OwnerId == z.OwnerId &&
                                                        oa.Value == orderById.ToString() &&
                                                        oa.Target.ValueType == "屬性"
                                                    ) &&
                                                    z.Target.ValueType == "存量"
                                                ) &&
                                                y.Target.ValueType == "存量"
                                            ).Select(y => y.Owner.No).OrderByDescending(y => y).FirstOrDefault(),
                                            Obj = new
                                            {
                                                id = x.Id,
                                                no = x.No,
                                                createdDate = x.CreatedDate,
                                                createdByNo = x.CreatedBy.No
                                            }
                                        }).OrderByDescending(x => x.Sort).Select(x => x.Obj).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                                        else jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                        {
                                            Sort = db.InventoryAttributes.Where(y =>
                                                y.TargetId == targetId &&
                                                db.InventoryAttributes.Any(z =>
                                                    z.TargetId == ownerId &&
                                                    z.Value == x.Id.ToString() &&
                                                    z.OwnerId == y.OwnerId &&
                                                    db.InventoryAttributes.Any(oa =>
                                                        oa.TargetId == ownerAttributeId &&
                                                        oa.OwnerId == z.OwnerId &&
                                                        oa.Value == orderById.ToString() &&
                                                        oa.Target.ValueType == "屬性"
                                                    ) &&
                                                    z.Target.ValueType == "存量"
                                                ) &&
                                                y.Target.ValueType == "存量"
                                            ).Select(y => y.Owner.No).OrderBy(y => y).FirstOrDefault(),
                                            Obj = new
                                            {
                                                id = x.Id,
                                                no = x.No,
                                                createdDate = x.CreatedDate,
                                                createdByNo = x.CreatedBy.No
                                            }
                                        }).OrderBy(x => x.Sort).Select(x => x.Obj).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                                        break;
                                    default:
                                        jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                        {
                                            id = x.Id,
                                            no = x.No,
                                            createdDate = x.CreatedDate,
                                            createdByNo = x.CreatedBy.No
                                        }).Take(pageSize.GetValueOrDefault()))));
                                        break;
                                }
                            }
                            else
                            {
                                if (descending) jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                {
                                    id = x.Id,
                                    no = x.No,
                                    createdDate = x.CreatedDate,
                                    createdByNo = x.CreatedBy.No
                                }).OrderByDescending(x => x.no).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                                else jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                                {
                                    id = x.Id,
                                    no = x.No,
                                    createdDate = x.CreatedDate,
                                    createdByNo = x.CreatedBy.No
                                }).OrderBy(x => x.no).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                            }
                        }
                        else
                        {
                            if (descending) jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                            {
                                id = x.Id,
                                no = x.No,
                                createdDate = x.CreatedDate,
                                createdByNo = x.CreatedBy.No
                            }).OrderByDescending(x => x.no).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                            else jObject.Add(new JProperty("result", JToken.FromObject(queriedInventories.Select(x => new
                            {
                                id = x.Id,
                                no = x.No,
                                createdDate = x.CreatedDate,
                                createdByNo = x.CreatedBy.No
                            }).OrderBy(x => x.no).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                        }
                        break;
                }
            }
            return Ok(jObject);
        }

        [Route("api/Inventories/Print")]
        public IQueryable<InventoryPrint> GetInventoryPrints(
            Guid? recipeId = null,
            Guid? itemId = null,
            Guid? inventoryId = null,
            int? pageIndex = 1,
            int? pageSize = 48
        )
        {
            return db.Inventories.Where(inventory =>
                inventory.No.ToString() != "" &&
            (inventory.ItemId == itemId || itemId == null) &&
            (inventory.Position.TargetId == inventoryId || inventoryId == null)
            ).OrderBy(x => x.No).Select(inventory => new InventoryPrint
            {
                Id = inventory.Id,
                No = inventory.No
            }).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault());
        }
         

        [Route("api/Inventories/WithAttributes")]
        public async Task<IHttpActionResult> PostInventoryWithAttributes(InventoryWithAttributes inventoryWithAttributes)
        {
            ICollection<InventoryAttribute> inventoryAttributes = inventoryWithAttributes.Attributes;
            Inventory inventory = new Inventory();
            inventory.Id = inventoryWithAttributes.Id;
            inventory.Position = inventoryWithAttributes.Position;
            inventory.ItemId = inventoryWithAttributes.ItemId;
            inventory.StatusId = inventoryWithAttributes.StatusId;
            inventory.No = inventoryWithAttributes.No;
            inventory.Value = inventoryWithAttributes.Value;
            db.Inventories.Add(inventory);
            await db.SaveChangesAsync();

            foreach (InventoryAttribute inventoryAttribute in inventoryAttributes)
            {
                if (inventoryAttribute.Value != null && inventoryAttribute.Value != "")
                {
                    if (inventoryAttribute.Target != null)
                    {
                        Entities.Attribute existedAttribute = await db.Attributes.FirstOrDefaultAsync(x => x.Value == inventoryAttribute.Target.Value);
                        if (existedAttribute != null)
                        {
                            inventoryAttribute.TargetId = existedAttribute.Id;
                            inventoryAttribute.Target = null;
                        }
                    }
                    db.InventoryAttributes.Add(inventoryAttribute);
                }
            }
            await db.SaveChangesAsync();

            return Ok(db.Inventories.Where(x => x.Id == inventoryWithAttributes.Id).Select(x => new
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
            }).FirstOrDefault(x => x.id == inventory.Id));
        }

        [Route("api/Inventories/WithAttributes/{id}")]
        public async Task<IHttpActionResult> PutInventoryWithAttributes(Guid id, InventoryWithAttributes inventoryWithAttributes)
        {
            ICollection<InventoryAttribute> inventoryAttributes = inventoryWithAttributes.Attributes;

            string no = inventoryWithAttributes.No;
            Inventory inventory = await db.Inventories.FindAsync(id);
            Inventory existedInventory = await db.Inventories.FindAsync(id);
            inventory.No = no;

            if (inventory.PositionId == null && inventoryWithAttributes.Position!=null)
            {
                db.Positions.Add(inventoryWithAttributes.Position);
                await db.SaveChangesAsync();
                inventory.PositionId = inventoryWithAttributes.Position.Id;

            }
            if (inventory.ItemId == null) inventory.ItemId = inventoryWithAttributes.ItemId;
            db.Entry(existedInventory).CurrentValues.SetValues(inventory);
            await db.SaveChangesAsync();
            foreach (InventoryAttribute inventoryAttribute in db.InventoryAttributes.Where(x=>x.OwnerId== inventory.Id).ToArray())
            {
                if(inventoryAttributes.FirstOrDefault(x=>x.Id== inventoryAttribute.Id) == null)
                {
                    db.InventoryAttributes.Remove(inventoryAttribute);
                }
            }
            await db.SaveChangesAsync();

            foreach (InventoryAttribute inventoryAttribute in inventoryAttributes)
            {
                InventoryAttribute existedInventoryAttribute = await db.InventoryAttributes.FindAsync(inventoryAttribute.Id);
                if (existedInventoryAttribute != null)
                {
                    if (inventoryAttribute.Value == null || inventoryAttribute.Value == "") db.InventoryAttributes.Remove(existedInventoryAttribute);
                    else db.Entry(existedInventoryAttribute).CurrentValues.SetValues(inventoryAttribute);
                }
                else
                {
                    if (inventoryAttribute.Target != null)
                    {

                        Entities.Attribute existedAttribute = await db.Attributes.FirstOrDefaultAsync(x => x.Value == inventoryAttribute.Target.Value);
                        if (existedAttribute != null)
                        {
                            inventoryAttribute.TargetId = existedAttribute.Id;
                            inventoryAttribute.Target = null;
                        }
                    }
                    db.InventoryAttributes.Add(inventoryAttribute);
                }
            }
            await db.SaveChangesAsync();
            return Ok(db.Inventories.Where(x => x.Id == id).Select(x => new
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
            }).FirstOrDefault(x => x.id == inventory.Id));
        }
        [Route("api/Inventories/Pickup")]
        public async Task<IHttpActionResult> PostInventoryPickup(InventoryPickup inventoryPickup)
        {
            string workOrderId = inventoryPickup.WorkOrderId.ToString();

            int pickupCount = db.Inventories.Where(z =>
                z.ItemId == inventoryPickup.ItemId &&
                db.Inventories.Any(x => x.Item.Code == "workOrderEvent" &&
                db.InventoryAttributes.Any(y => y.OwnerId == x.Id && y.Target.Code == "eventType" && y.Value == "pickup") &&
                db.InventoryAttributes.Any(y => y.OwnerId == x.Id && y.Target.Code == "workOrder" && y.Value == workOrderId) &&
                db.InventoryAttributes.Any(y => y.OwnerId == x.Id && y.Target.Code == "inventoryIds" && y.Value == z.Id.ToString())
            )).Count() + 1;

            Item item = db.Items.Find(inventoryPickup.ItemId);
            Inventory workOrder = db.Inventories.Find(inventoryPickup.WorkOrderId);
            float? value = inventoryPickup.Segmentations.Sum(x => x.Quantity);
            Inventory inventory = new Inventory
            {
                ItemId = inventoryPickup.ItemId,
                Value = value,
                No = "工單【" + workOrder.No + "】的【" + item.Value + "】第" + pickupCount.ToString() + "次領料",
                Position = new Position
                {
                    StartDate = DateTime.Now,
                    TargetId = inventoryPickup.TargetId
                }
            };
            db.Inventories.Add(inventory);
            await db.SaveChangesAsync();

            foreach (Segmentation segmentation in inventoryPickup.Segmentations)
            {
                segmentation.TargetId = inventory.Id;
                db.Segmentations.Add(segmentation);
            }
            await db.SaveChangesAsync();

            Position existedPosition = await db.Positions.FindAsync(inventory.PositionId);
            inventory.Position.OwnerId = inventory.Id;
            db.Entry(existedPosition).CurrentValues.SetValues(inventory.Position);
            await db.SaveChangesAsync();

            Guid workOredrEventId = new Guid("f318dbd8-d500-4390-b9c1-2f112c157d1a");
            Inventory workOredrEvent = new Inventory
            {
                Id = inventoryPickup.Id,
                ItemId = workOredrEventId,
                Value = 1,
                No = "工單【" + workOrder.No + "】的【" + item.Value + "】第" + pickupCount.ToString() + "次領料事件"
            };
            db.Inventories.Add(workOredrEvent);
            await db.SaveChangesAsync();

            if (workOrderId.Length == 36)
            {
                Guid targetId = new Guid("592473a1-95d0-49cd-87bf-120c5f682204");
                InventoryAttribute workOredr = new InventoryAttribute
                {
                    OwnerId = workOredrEvent.Id,
                    TargetId = targetId,
                    Value = workOrderId
                };
                db.InventoryAttributes.Add(workOredr);
            }
            if ("pickup" != "")
            {
                Guid targetId = new Guid("c144726c-16f9-477d-ae85-610303dbf0a1");
                InventoryAttribute eventType = new InventoryAttribute
                {
                    OwnerId = workOredrEvent.Id,
                    TargetId = targetId,
                    Value = "pickup"
                };
                db.InventoryAttributes.Add(eventType);
            }

            string pickupInventoryIds = inventory.Id.ToString();
            if (inventory.PositionId.ToString().Length == 36)
            {
                Guid targetId = new Guid("94c2ea86-ae75-47ca-a725-d385beba03ea");
                InventoryAttribute inventoryIds = new InventoryAttribute
                {
                    OwnerId = workOredrEvent.Id,
                    TargetId = targetId,
                    Value = pickupInventoryIds
                };
                db.InventoryAttributes.Add(inventoryIds);
            }

            if (inventoryPickup.Memo != null)
            {
                Guid targetId = new Guid("f7432ea6-33e1-4a8c-b390-010e0091833e");
                InventoryAttribute memo = new InventoryAttribute
                {
                    OwnerId = workOredrEvent.Id,
                    TargetId = targetId,
                    Value = inventoryPickup.Memo
                };
                db.InventoryAttributes.Add(memo);
            }
            await db.SaveChangesAsync();

            return Ok(db.Inventories.Where(x => x.Id == workOredrEvent.Id).Select(x => new
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
            }).FirstOrDefault(x => x.id == workOredrEvent.Id));
        }

        [Route("api/Inventories/Startup")]
        public async Task<IHttpActionResult> PostInventoryStartup(InventoryStartup inventoryStartup)
        {
            string workOrderId = inventoryStartup.WorkOrderId.ToString();

            int startupCount = db.Positions.Where(z =>
                z.Owner.ItemId == inventoryStartup.ItemId &&
                db.Inventories.Any(x => x.Item.Code == "workOrderEvent" &&
                db.InventoryAttributes.Any(y => y.OwnerId == x.Id && y.Target.Code == "eventType" && y.Value == "startup") &&
                db.InventoryAttributes.Any(y => y.OwnerId == x.Id && y.Target.Code == "workOrder" && y.Value == workOrderId) &&
                db.InventoryAttributes.Any(y => y.OwnerId == x.Id && y.Target.Code == "inventoryIds" && y.Value == z.Id.ToString())
            )).Count() + 1;

            Item item = db.Items.Find(inventoryStartup.ItemId);
            Inventory workOrder = db.Inventories.Find(inventoryStartup.WorkOrderId);
            float? value = inventoryStartup.UpValue;
            Inventory inventory = new Inventory
            {
                ItemId = inventoryStartup.ItemId,
                Value = value,
                No = "工單【" + workOrder.No + "】的【" + item.Value + "】第" + startupCount.ToString() + "次投產",
                Position = new Position
                {
                    StartDate = DateTime.Now,
                    TargetId = inventoryStartup.TargetId
                }
            };
            db.Inventories.Add(inventory);
            await db.SaveChangesAsync();


            var segmentationOwners = db.Inventories.Where(z =>
                z.ItemId == inventoryStartup.ItemId &&
                db.Inventories.Any(x => x.Item.Code == "workOrderEvent" &&
                db.InventoryAttributes.Any(y => y.OwnerId == x.Id && y.Target.Code == "eventType" && y.Value == "pickup") &&
                db.InventoryAttributes.Any(y => y.OwnerId == x.Id && y.Target.Code == "workOrder" && y.Value == workOrderId) &&
                db.InventoryAttributes.Any(y => y.OwnerId == x.Id && y.Target.Code == "inventoryIds" && y.Value == z.Id.ToString())
            )).Select(x => new
            {
                x.Id,
                Value = x.Value - (db.Segmentations.Where(y => y.OwnerId == x.Id).Sum(y => y.Quantity) ?? 0),
                x.CreatedDate
            }).OrderBy(x => x.CreatedDate).ToList();




            float? left = value;
            foreach (var segmentationOwner in segmentationOwners)
            {
                if (left > 0 && segmentationOwner.Value > 0)
                {
                    Segmentation segmentation = new Segmentation
                    {
                        OwnerId = segmentationOwner.Id,
                        TargetId = inventory.Id
                    };

                    if (segmentationOwner.Value > left)
                    {
                        segmentation.Quantity = left;
                        segmentation.OwnerId = segmentationOwner.Id;
                    }
                    else
                    {
                        segmentation.Quantity = segmentationOwner.Value;
                    }
                    left = left - segmentationOwner.Value;
                    db.Segmentations.Add(segmentation);
                }
            }
            await db.SaveChangesAsync();

            Position existedPosition = await db.Positions.FindAsync(inventory.PositionId);
            inventory.Position.OwnerId = inventory.Id;
            db.Entry(existedPosition).CurrentValues.SetValues(inventory.Position);
            await db.SaveChangesAsync();

            Guid workOredrEventId = new Guid("f318dbd8-d500-4390-b9c1-2f112c157d1a");
            Inventory workOredrEvent = new Inventory
            {
                ItemId = workOredrEventId,
                Value = 1,
                No = "工單【" + workOrder.No + "】的【" + item.Value + "第】" + startupCount.ToString() + "次投產事件"
            };
            db.Inventories.Add(workOredrEvent);
            await db.SaveChangesAsync();

            if (workOrderId.Length == 36)
            {
                Guid targetId = new Guid("592473a1-95d0-49cd-87bf-120c5f682204");
                InventoryAttribute workOredr = new InventoryAttribute
                {
                    OwnerId = workOredrEvent.Id,
                    TargetId = targetId,
                    Value = workOrderId
                };
                db.InventoryAttributes.Add(workOredr);
            }
            if ("startup" != "")
            {
                Guid targetId = new Guid("c144726c-16f9-477d-ae85-610303dbf0a1");
                InventoryAttribute eventType = new InventoryAttribute
                {
                    OwnerId = workOredrEvent.Id,
                    TargetId = targetId,
                    Value = "startup"
                };
                db.InventoryAttributes.Add(eventType);
            }

            string startupInventoryIds = inventory.Id.ToString();
            if (startupInventoryIds.Length == 36)
            {
                Guid targetId = new Guid("94c2ea86-ae75-47ca-a725-d385beba03ea");
                InventoryAttribute inventoryIds = new InventoryAttribute
                {
                    OwnerId = workOredrEvent.Id,
                    TargetId = targetId,
                    Value = startupInventoryIds
                };
                db.InventoryAttributes.Add(inventoryIds);
            }

            if (inventoryStartup.Memo != null)
            {
                Guid targetId = new Guid("f7432ea6-33e1-4a8c-b390-010e0091833e");
                InventoryAttribute memo = new InventoryAttribute
                {
                    OwnerId = workOredrEvent.Id,
                    TargetId = targetId,
                    Value = inventoryStartup.Memo
                };
                db.InventoryAttributes.Add(memo);
            }
            await db.SaveChangesAsync();



            return Ok(db.Inventories.Where(x => x.Id == workOredrEvent.Id).Select(x => new
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
            }).FirstOrDefault(x => x.id == workOredrEvent.Id));
        }

        [Route("api/Inventories/Inspection")]
        public async Task<IHttpActionResult> PostInventoryInspection(InventoryInspection inventoryInspection)
        {
            string workOrderId = inventoryInspection.WorkOrderId.ToString();

            int inspectionCount = db.Inventories.Where(z => z.ItemId == inventoryInspection.ItemId &&
                db.Inventories.Any(x => x.Item.Code == "workOrderEvent" &&
                db.InventoryAttributes.Any(y => y.OwnerId == x.Id && y.Target.Code == "eventType" && y.Value == "inspection") &&
                db.InventoryAttributes.Any(y => y.OwnerId == x.Id && y.Target.Code == "workOrder" && y.Value == workOrderId) &&
                db.InventoryAttributes.Any(y => y.OwnerId == x.Id && y.Target.Code == "inventoryIds" && y.Value.Substring(0, 36) == z.Id.ToString())
            )).Count() + 1;


            Item item = db.Items.Find(inventoryInspection.ItemId);
            float value = inventoryInspection.UpValue - inventoryInspection.Defects.Sum(x => x.Value);
            Inventory workOrder = db.Inventories.Find(inventoryInspection.WorkOrderId);
            Inventory inventory = new Inventory
            {
                ItemId = inventoryInspection.ItemId,
                Value = value,
                No = "工單【" + workOrder.No + "】的【" + item.Value + "】第" + inspectionCount.ToString() + "次產出",
                Position = new Position
                {
                    StartDate = DateTime.Now,
                    TargetId = inventoryInspection.TargetId
                }
            };
            db.Inventories.Add(inventory);
            await db.SaveChangesAsync();
            Position existedPosition = await db.Positions.FindAsync(inventory.PositionId);
            inventory.Position.OwnerId = inventory.Id;
            db.Entry(existedPosition).CurrentValues.SetValues(inventory.Position);
            await db.SaveChangesAsync();


            #region inspectionInventoryIds
            string inspectionInventoryIds = inventory.Id.ToString().ToLower();
            foreach (Defect defect in inventoryInspection.Defects)
            {
                Inventory defectInventory = new Inventory
                {
                    ItemId = inventoryInspection.ItemId,
                    Value = defect.Value,
                    No = "工單【" + workOrder.No + "】的【" + item.Value + "】第" + inspectionCount.ToString() + "次產出(" + defect.AttributeValue + ")",
                    Position = new Position
                    {
                        StartDate = DateTime.Now,
                        TargetId = inventoryInspection.TargetId
                    }
                };
                db.Inventories.Add(defectInventory);
                await db.SaveChangesAsync();
                inspectionInventoryIds = inspectionInventoryIds + "," + defectInventory.Id.ToString().ToLower();
                string[] defectAttributeValues = defect.AttributeValue.Split(',');
                foreach (string defectAttributeValue in defectAttributeValues)
                {
                    string attributeValue = defectAttributeValue.Trim();
                    Entities.Attribute attribute = db.Attributes.FirstOrDefault(x => x.Value == attributeValue);
                    InventoryAttribute defectIs = new InventoryAttribute
                    {
                        OwnerId = defectInventory.Id,
                        Value = "1"
                    };
                    if (attribute != null)
                    {
                        defectIs.TargetId = attribute.Id;
                    }
                    else
                    {
                        Guid statusId = new Guid("005617b3-d283-461c-abef-5c0c16c780d0");
                        defectIs.Target = new Entities.Attribute
                        {
                            StatusId = statusId,
                            Value = attributeValue,
                            Code = attributeValue,
                            Level = 3,
                            ValueNumber = 1,
                            ValueType = "數值",
                            ItemIds = "",
                            InventoryIds = "",
                            CategoryIds = ""
                        };
                    }
                    db.InventoryAttributes.Add(defectIs);
                }
                Position existedDefectPosition = await db.Positions.FindAsync(defectInventory.PositionId);
                defectInventory.Position.OwnerId = defectInventory.Id;
                db.Entry(existedDefectPosition).CurrentValues.SetValues(defectInventory.Position);
            }
            await db.SaveChangesAsync();
            #endregion

            Guid workOredrEventId = new Guid("f318dbd8-d500-4390-b9c1-2f112c157d1a");
            Inventory workOredrEvent = new Inventory
            {
                ItemId = workOredrEventId,
                Value = 1,
                No = "工單" + workOrder.No + "的" + item.Value + "第" + inspectionCount.ToString() + "次產出事件"
            };
            db.Inventories.Add(workOredrEvent);
            await db.SaveChangesAsync();



            if (workOrderId.Length == 36)
            {
                Guid targetId = new Guid("592473a1-95d0-49cd-87bf-120c5f682204");
                InventoryAttribute workOredr = new InventoryAttribute
                {
                    OwnerId = workOredrEvent.Id,
                    TargetId = targetId,
                    Value = workOrderId
                };
                db.InventoryAttributes.Add(workOredr);
            }
            if ("inspection" != "")
            {
                Guid targetId = new Guid("c144726c-16f9-477d-ae85-610303dbf0a1");
                InventoryAttribute eventType = new InventoryAttribute
                {
                    OwnerId = workOredrEvent.Id,
                    TargetId = targetId,
                    Value = "inspection"
                };
                db.InventoryAttributes.Add(eventType);
            }

            if (inspectionInventoryIds.Length >= 36)
            {
                Guid targetId = new Guid("94c2ea86-ae75-47ca-a725-d385beba03ea");
                InventoryAttribute inventoryIds = new InventoryAttribute
                {
                    OwnerId = workOredrEvent.Id,
                    TargetId = targetId,
                    Value = inspectionInventoryIds
                };
                db.InventoryAttributes.Add(inventoryIds);
            }

            if (inventoryInspection.Memo != null)
            {
                Guid targetId = new Guid("f7432ea6-33e1-4a8c-b390-010e0091833e");
                InventoryAttribute memo = new InventoryAttribute
                {
                    OwnerId = workOredrEvent.Id,
                    TargetId = targetId,
                    Value = inventoryInspection.Memo
                };
                db.InventoryAttributes.Add(memo);
            }


            await db.SaveChangesAsync();

            return Ok(db.Inventories.Where(x => x.Id == workOredrEvent.Id).Select(x => new
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
            }).FirstOrDefault(x => x.id == workOredrEvent.Id));
        }

        [Route("api/Inventories/Station")]
        public async Task<IHttpActionResult> PostInventoryStation(InventoryStation inventoryStation)
        {
            string stationIdIs = inventoryStation.TargetId.ToString();
            string workOrderId = inventoryStation.WorkOrderId.ToString();
            string actionValue = inventoryStation.Action;
            int stationCount = db.Inventories.Where(x => x.Item.Code == "workOrderEvent" &&
                db.InventoryAttributes.Any(y => y.OwnerId == x.Id && y.Target.Code == "eventType" && y.Value == "station") &&
                db.InventoryAttributes.Any(y => y.OwnerId == x.Id && y.Target.Code == "workOrder" && y.Value == workOrderId) &&
                db.InventoryAttributes.Any(y => y.OwnerId == x.Id && y.Target.Code == "action" && y.Value == actionValue)
            ).Count() + 1;
            Inventory station = db.Inventories.Find(inventoryStation.TargetId);
            Inventory workOrder = db.Inventories.Find(inventoryStation.WorkOrderId);



            Guid workOredrEventId = new Guid("f318dbd8-d500-4390-b9c1-2f112c157d1a");
            Inventory workOredrEvent = new Inventory
            {
                ItemId = workOredrEventId,
                Value = 1,
                No = "工單【" + workOrder.No + "】的【" + station.No + "】第" + stationCount.ToString() + "次" + actionValue
            };
            db.Inventories.Add(workOredrEvent);
            await db.SaveChangesAsync();



            if (workOrderId.Length == 36)
            {
                Guid targetId = new Guid("592473a1-95d0-49cd-87bf-120c5f682204");
                InventoryAttribute workOredr = new InventoryAttribute
                {
                    OwnerId = workOredrEvent.Id,
                    TargetId = targetId,
                    Value = workOrderId
                };
                db.InventoryAttributes.Add(workOredr);
            }
            if ("station" != "")
            {
                Guid targetId = new Guid("c144726c-16f9-477d-ae85-610303dbf0a1");
                InventoryAttribute eventType = new InventoryAttribute
                {
                    OwnerId = workOredrEvent.Id,
                    TargetId = targetId,
                    Value = "station"
                };
                db.InventoryAttributes.Add(eventType);
            }
            if (inventoryStation.Action != null && inventoryStation.Action.ToString() != "")
            {
                Guid targetId = new Guid("93a7862f-aece-45e7-9c3c-9a8265a5fa0c");
                InventoryAttribute action = new InventoryAttribute
                {
                    OwnerId = workOredrEvent.Id,
                    TargetId = targetId,
                    Value = inventoryStation.Action
                };
                db.InventoryAttributes.Add(action);
            }
            if (inventoryStation.UsedPower != null && Convert.ToDouble(inventoryStation.UsedPower) > 0)
            {
                Guid targetId = new Guid("2067a80e-eb80-4640-b9e3-21ee26e64e1b");
                InventoryAttribute usedPower = new InventoryAttribute
                {
                    OwnerId = workOredrEvent.Id,
                    TargetId = targetId,
                    Value = inventoryStation.UsedPower.ToString()
                };
                db.InventoryAttributes.Add(usedPower);
            }

            if (inventoryStation.Memo != null && inventoryStation.Memo.ToString() != "")
            {
                Guid targetId = new Guid("f7432ea6-33e1-4a8c-b390-010e0091833e");
                InventoryAttribute memo = new InventoryAttribute
                {
                    OwnerId = workOredrEvent.Id,
                    TargetId = targetId,
                    Value = inventoryStation.Memo
                };
                db.InventoryAttributes.Add(memo);
            }

            if (stationIdIs.Length == 36)
            {
                Guid targetId = new Guid("6830b8a5-8be4-419c-a433-3caa70fe73e9");
                InventoryAttribute stationId = new InventoryAttribute
                {
                    OwnerId = workOredrEvent.Id,
                    TargetId = targetId,
                    Value = stationIdIs
                };
                db.InventoryAttributes.Add(stationId);
            }

            foreach (Reason reason in inventoryStation.Reasons)
            {
                string attributeValue = reason.Value.Trim();
                Entities.Attribute attribute = db.Attributes.FirstOrDefault(x => x.Value == attributeValue);
                InventoryAttribute reasonIs = new InventoryAttribute
                {
                    OwnerId = workOredrEvent.Id,
                    Value = "1"
                };
                if (attribute != null)
                {
                    reasonIs.TargetId = attribute.Id;
                }
                else
                {
                    Guid statusId = new Guid("005617b3-d283-461c-abef-5c0c16c780d0");
                    reasonIs.Target = new Entities.Attribute
                    {
                        StatusId = statusId,
                        Value = attributeValue,
                        Code = attributeValue,
                        Level = 3,
                        ValueNumber = 1,
                        ValueType = "數值",
                        ItemIds = "",
                        InventoryIds = "",
                        CategoryIds = ""
                    };
                }
                db.InventoryAttributes.Add(reasonIs);
            }
            await db.SaveChangesAsync();

            return Ok(db.Inventories.Where(x => x.Id == workOredrEvent.Id).Select(x => new
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
                positionStartDate = x.Position.StartDate,
                createdDate = x.CreatedDate,
                createdByNo = x.CreatedBy.No
            }).FirstOrDefault(x => x.id == workOredrEvent.Id));
        }











        public async Task<IHttpActionResult> PutInventory(Guid id, Inventory inventory)
        {
            Inventory existedInventory = await db.Inventories.FindAsync(id);

            if (inventory.Position != null)
            {
                inventory.Position.OwnerId = inventory.Id;
                Position existedPosition = await db.Positions.FindAsync(inventory.Position.Id);
                if (existedPosition != null)
                {
                    db.Entry(existedPosition).CurrentValues.SetValues(inventory.Position);
                }
                else
                {
                    db.Positions.Add(inventory.Position);
                }
                await db.SaveChangesAsync();
                inventory.PositionId = inventory.Position.Id;
            }
            db.Entry(existedInventory).CurrentValues.SetValues(inventory);
            await db.SaveChangesAsync();
            return Ok(db.Inventories.Where(x => x.Id == inventory.Id).Select(x => new
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
            }).FirstOrDefault(x => x.id == inventory.Id));
        }


        [Route("api/Inventories/Select")]
        public IQueryable<InventorySelect> GetInventorySelects(
            string anyLike = ""
        )
        {
            if (anyLike == null) anyLike = "";
            else anyLike = HttpUtility.UrlDecode(anyLike);
            return db.Inventories.Where(x =>
              x.Value.ToString().Contains(anyLike)
            ).Select(inventory => new InventorySelect
            {
                Id = inventory.Id,
                No = inventory.No,

                SubValue = db.Positions.Where(position => position.TargetId == inventory.Id && position.EndDate == null).Count().ToString()
            }).OrderBy(x => x.No);
        }

        public async Task<IHttpActionResult> DeleteInventory(Guid id)
        {
            HashSet<Guid> deleteIds = new HashSet<Guid>();
            Inventory inventory = await db.Inventories.FindAsync(id);
            if (inventory == null) return NotFound();
            Segmentation segmentation = await db.Segmentations.FirstOrDefaultAsync(x => x.TargetId == id);
            if (segmentation != null)
            {
                Inventory segmentationOwner = await db.Inventories.FindAsync(segmentation.OwnerId);
                if (segmentationOwner != null)
                {
                    Inventory existedSegmentationOwner = await db.Inventories.FindAsync(segmentationOwner.Id);
                    segmentationOwner.Value -= segmentation.Quantity;
                    segmentationOwner.Version++;
                    db.Entry(existedSegmentationOwner).CurrentValues.SetValues(segmentationOwner);
                }
                Segmentation existedSegmentation = await db.Segmentations.FindAsync(segmentation.Id);
                db.Segmentations.Remove(segmentation);
            }
            Inventory existedInventory = await db.Inventories.FindAsync(inventory.Id);
            inventory.PositionId = null;
            db.Entry(existedInventory).CurrentValues.SetValues(inventory);
            await db.SaveChangesAsync();

            foreach (Segmentation ownerSegmentation in db.Segmentations.Where(x => x.OwnerId == id).ToArray())
            {
                db.Segmentations.Remove(ownerSegmentation);
            }
            foreach (InventoryAttribute inventoryAttribute in db.InventoryAttributes.Where(x => x.OwnerId == id).ToArray())
            {
                db.InventoryAttributes.Remove(inventoryAttribute);
            }
            foreach (Position inventoryPosition in db.Positions.Where(x => x.OwnerId == id).ToArray())
            {
                db.Positions.Remove(inventoryPosition);
            }
            db.Inventories.Remove(inventory);
            await db.SaveChangesAsync();


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
            return Ok();
        }
    }
}