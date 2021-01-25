using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HoYa.Entities;
using System;
using HoYa.Models;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Web;
using Newtonsoft.Json.Linq;

namespace HoYa.Controllers
{

    public class ItemsController : ApiController
    {
        private HoYaContext db = new HoYaContext();

        public async Task<IHttpActionResult> GetItem(Guid id)
        {
            IQueryable<Item> items = db.Items.Where(x => x.Id == id);


            if (items.Count() == 0)
            {
                return NotFound();
            }

            return Ok(items.Select(x => new
            {
                id = x.Id,
                value = x.Value,
                photo = x.Photo.Target.Path,
                code = x.Code,
                description = x.Description,
                deletable = (db.Inventories.Where(y => y.ItemId == x.Id).Count() == 0),
                statusId=x.StatusId
            }).FirstOrDefault());
        }

        public async Task<IHttpActionResult> GetItems(
            string anyLike = "",
            int? take = 5,
            Guid? categoryId = null,
            string categoryIds = ""
        )
        {
            if (anyLike == null) anyLike = "";
            else anyLike = HttpUtility.UrlDecode(anyLike);
            IQueryable<Item> items = db.Items;

            if (categoryIds != "")
            {
                IQueryable<string> _categoryIds = categoryIds.Split(',').AsQueryable();
                if (_categoryIds.Count() > 1) items = db.ItemCategories.Where(x => _categoryIds.Contains(x.TargetId.ToString())).Select(x => x.Owner).Distinct();
                else
                {
                    if (categoryId == null && _categoryIds.FirstOrDefault().Length == 36) categoryId = Guid.Parse(_categoryIds.FirstOrDefault());
                }
            }

            if (categoryId != null)
            {
                items = db.ItemCategories.Where(x => x.TargetId == categoryId).Select(x => x.Owner);
            }

            if (anyLike != "") items = items.Where(x =>
                x.Value.Contains(anyLike) ||
                x.Code.ToString().Contains(anyLike) ||
                x.Description.ToString().Contains(anyLike)
            );
            return Ok(items.Take(take.GetValueOrDefault()).Select(x => new { id = x.Id, value = x.Value, code = x.Code }));
        }

        [Route("api/Items/Detail")]
        public IQueryable<ItemDetail> GetItemDetails(
            string anyLike = "",
            int? take = 5,
            Guid? categoryId = null
        )
        {
            IQueryable<Item> items = db.Items;
            if (categoryId != null) items = items.Where(x => db.ItemCategories.Count(y => y.OwnerId == x.Id && y.TargetId == categoryId && y.EndDate == null) > 0);

            if (anyLike == null) anyLike = "";
            else anyLike = HttpUtility.UrlDecode(anyLike);
            return items.Where(x =>
                x.Value.ToString().Contains(anyLike) ||
                x.Code.ToString().Contains(anyLike) ||
                x.Description.ToString().Contains(anyLike)
            ).Select(x => new ItemDetail
            {
                Id = x.Id,
                Code = x.Code,
                Value = x.Value,
                Photo = x.Photo.Target.Path,
                Description = x.Description,
                Deletable = (db.Inventories.Where(y => y.ItemId == x.Id).Count() == 0)
            }).Take(take.GetValueOrDefault());
        }

        [Route("api/Items/Count")]
        public async Task<IHttpActionResult> GetItemsCount(
            string value = "",
            string code = "",
            Guid? statusId = null,
            Guid? excludeId = null,
            Guid? createdById = null
        )
        {
            if (value == null) value = "";
            else value = HttpUtility.UrlDecode(value);
            if (code == null) code = "";
            else code = HttpUtility.UrlDecode(code);
            Guid exclude = excludeId.GetValueOrDefault();
            return Ok(db.Items.Where(x =>
            (x.Id != exclude || excludeId == null) &&
            (x.StatusId == statusId || statusId == null) &&
            (x.Value == value || value == "") &&
            (x.Code == code || code == "") &&
            (x.CreatedById == createdById || createdById == null) &&
            (!(value == "" && code == "") || excludeId == null)
            ).Count());
        }

        [Route("api/Items/IsDuplicate")]
        public async Task<IHttpActionResult> GetItemIsDuplicate(
            Guid excludeId,
            string value = "",
            string code = ""
        )
        {
            if (value == null) value = "";
            else value = HttpUtility.UrlDecode(value);
            if (code == null) code = "";
            else code = HttpUtility.UrlDecode(code);
            if (value != "")
            {
                Item item = db.Items.FirstOrDefault(x => x.Value == value && x.Id != excludeId);
                if (item != null) return Ok(true);
            }
            if (code != "")
            {
                Item item = db.Items.FirstOrDefault(x => x.Code == code && x.Id != excludeId);
                if (item != null) return Ok(true);
            }
            return Ok(false);
        }



        [Route("api/Items/By")]
        public async Task<IHttpActionResult> GetItemBy(
            string value = "",
            string code = ""
        )
        {
            if (value == null) value = "";
            else value = HttpUtility.UrlDecode(value);
            if (code == null) code = "";
            else code = HttpUtility.UrlDecode(code);
            return Ok(await db.Items.FirstOrDefaultAsync(x =>
            (x.Value == value || value == "") &&
            (x.Code == code || code == "")
            ));
        }




        [Route("api/Items/Detail/{id}")]
        public async Task<IHttpActionResult> GetItemDetail(Guid id)
        {
            ItemDetail itemList = await db.Items.Where(x => x.Id == id).Select(x => new ItemDetail
            {
                Id = x.Id,
                Value = x.Value,
                Photo = x.Photo.Target.Path,
                Description = x.Description,
                Deletable = (db.Inventories.Where(y => y.ItemId == x.Id).Count() == 0),
                Status = x.Status.Value
            }).FirstOrDefaultAsync();
            if (itemList == null)
            {
                return NotFound();
            }

            return Ok(itemList);
        }

        [Route("api/Items/List")]
        public async Task<IHttpActionResult> GetItemLists(
            string anyLike = "",
            int? take = 200,
            Guid? categoryId = null,
            string orderBy = "",
            int? pageSize = 20,
            int? pageIndex = 1,
            bool descending = false
        )
        {
            JObject jObject = new JObject();
            IQueryable<Item> items;
            if (categoryId != null)
            {
                items = db.Items.Where(item => item.Value != "" && db.ItemCategories.Any(itemCategory => itemCategory.OwnerId == item.Id && itemCategory.TargetId == categoryId));
            }
            else
            {
                items = db.Items.Where(item => item.Value != "");
            }

            if (anyLike == null) anyLike = "";
            else anyLike = HttpUtility.UrlDecode(anyLike);
            items = items.Where(x =>
                              x.Value.ToString().Contains(anyLike) ||
                              x.Code.ToString().Contains(anyLike) ||
                              x.Description.ToString().Contains(anyLike)
                        );
            jObject.Add(new JProperty("total", items.Count()));
            switch (orderBy)
            {
                case "value":
                    if (descending)
                        jObject.Add(new JProperty("result", JToken.FromObject(items.Select(x => new
                        {
                            id = x.Id,
                            value = x.Value,
                            photo = x.Photo.Target.Path,
                            code = x.Code,
                            description = x.Description,
                            deletable = (db.Inventories.Where(y => y.ItemId == x.Id).Count() == 0)
                        }).OrderByDescending(x => x.value).Take(take.GetValueOrDefault()).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                    else
                        jObject.Add(new JProperty("result", JToken.FromObject(items.Select(x => new
                        {
                            id = x.Id,
                            value = x.Value,
                            photo = x.Photo.Target.Path,
                            code = x.Code,
                            description = x.Description,
                            deletable = (db.Inventories.Where(y => y.ItemId == x.Id).Count() == 0)
                        }).OrderBy(x => x.value).Take(take.GetValueOrDefault()).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                    break;
                case "code":
                    if (descending)
                        jObject.Add(new JProperty("result", JToken.FromObject(items.Select(x => new
                        {
                            id = x.Id,
                            value = x.Value,
                            photo = x.Photo.Target.Path,
                            code = x.Code,
                            description = x.Description,
                            deletable = (db.Inventories.Where(y => y.ItemId == x.Id).Count() == 0)
                        }).OrderByDescending(x => x.code).Take(take.GetValueOrDefault()).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                    else
                        jObject.Add(new JProperty("result", JToken.FromObject(items.Select(x => new
                        {
                            id = x.Id,
                            value = x.Value,
                            photo = x.Photo.Target.Path,
                            code = x.Code,
                            description = x.Description,
                            deletable = (db.Inventories.Where(y => y.ItemId == x.Id).Count() == 0)
                        }).OrderBy(x => x.code).Take(take.GetValueOrDefault()).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                    break;
                case "description":
                    if (descending)
                        jObject.Add(new JProperty("result", JToken.FromObject(items.Select(x => new
                        {
                            id = x.Id,
                            value = x.Value,
                            photo = x.Photo.Target.Path,
                            code = x.Code,
                            description = x.Description,
                            deletable = (db.Inventories.Where(y => y.ItemId == x.Id).Count() == 0)
                        }).OrderByDescending(x => x.description).Take(take.GetValueOrDefault()).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                    else
                        jObject.Add(new JProperty("result", JToken.FromObject(items.Select(x => new
                        {
                            id = x.Id,
                            value = x.Value,
                            photo = x.Photo.Target.Path,
                            code = x.Code,
                            description = x.Description,
                            deletable = (db.Inventories.Where(y => y.ItemId == x.Id).Count() == 0)
                        }).OrderBy(x => x.description).Take(take.GetValueOrDefault()).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                    break;
                default:
                    if (descending)
                        jObject.Add(new JProperty("result", JToken.FromObject(items.Select(x => new
                        {
                            id = x.Id,
                            value = x.Value,
                            photo = x.Photo.Target.Path,
                            code = x.Code,
                            description = x.Description,
                            deletable = (db.Inventories.Where(y => y.ItemId == x.Id).Count() == 0)
                        }).OrderByDescending(x => x.value).Take(take.GetValueOrDefault()).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                    else
                        jObject.Add(new JProperty("result", JToken.FromObject(items.Select(x => new
                        {
                            id = x.Id,
                            value = x.Value,
                            photo = x.Photo.Target.Path,
                            code = x.Code,
                            description = x.Description,
                            deletable = (db.Inventories.Where(y => y.ItemId == x.Id).Count() == 0)
                        }).OrderBy(x => x.value).Take(take.GetValueOrDefault()).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()))));
                    break;
            }
            return Ok(jObject);
        }

        [Route("api/Items/Select")]
        public IQueryable<ItemSelect> GetItemSelects(
            string anyLike = "",
            Guid? statusId = null
        )
        {
            statusId = new Guid("005617B3-D283-461C-ABEF-5C0C16C780D0");
            if (anyLike == null) anyLike = "";
            else anyLike = HttpUtility.UrlDecode(anyLike);
            return db.Items.Where(item =>
            item.StatusId == statusId &&
            (item.Value.ToString().Contains(anyLike) ||
            item.Code.ToString().Contains(anyLike)) &&

            item.Code != null
            ).Select(item => new ItemSelect
            {
                Id = item.Id,
                Code = item.Code,
                Value = item.Value,
                Photo = item.Photo.Target.Path

            }).OrderBy(x => x.Value).Take(4);
        }

        [Route("api/Items/Query")]
        public async Task<IHttpActionResult> GetItemsQuery(
           string anyLike = "",
           string sortBy = "",
           string orderBy = "",
           int? pageIndex = 1,
           int? pageSize = 20
        )
        {
            IQueryable<Item> items = db.Items.Where(x => (x.Value.ToString().Contains(anyLike) || anyLike == null) ||
                           (x.Code.ToString().Contains(anyLike) || anyLike == null));
            switch (sortBy)
            {
                default:
                    if (orderBy == "asc") return Ok(new Query<Item>
                    {
                        PaginatorLength = items.Count(),
                        Data = items.OrderBy(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<Item>()
                    });
                    else return Ok(new Query<Item>
                    {
                        PaginatorLength = items.Count(),
                        Data = items.OrderByDescending(x => x.CreatedDate).Skip((pageIndex.GetValueOrDefault() - 1) * pageSize.GetValueOrDefault()).Take(pageSize.GetValueOrDefault()).ToHashSet<Item>()
                    });
            }
        }

        public async Task<IHttpActionResult> PutItem(Guid id, Item item)
        {
            HashSet<Category> categories = new HashSet<Category>();
            if (item.Description != null)
            {
                string[] description = item.Description.Split('#');
                foreach (string _categoryValue in description)
                {
                    string categoryValue = _categoryValue.Trim();
                    if (categoryValue != "" && categories.Count(x => x.Value == categoryValue) == 0)
                    {
                        Category category = await db.Categories.FirstOrDefaultAsync(x => x.Value == categoryValue.Trim());
                        if (category == null)
                        {
                            category = new Category { Value = categoryValue.Trim() };
                            db.Categories.Add(category);
                            await db.SaveChangesAsync();
                        }
                        categories.Add(category);
                    }
                }
            }

            foreach (Category category in categories)
            {
                ItemCategory itemCategory = await db.ItemCategories.FirstOrDefaultAsync(x => x.OwnerId == id && x.TargetId == category.Id);
                if (itemCategory == null)
                {
                    db.ItemCategories.Add(new ItemCategory
                    {
                        StartDate = DateTime.Now,
                        OwnerId = item.Id,
                        TargetId = category.Id

                    });
                    await db.SaveChangesAsync();
                }
            }
            foreach (ItemCategory itemCategory in db.ItemCategories.Where(x => x.OwnerId == id).ToArray())
            {
                Category category = categories.FirstOrDefault(x => x.Id == itemCategory.TargetId);
                if (category == null)
                {
                    db.ItemCategories.Remove(itemCategory);
                }
            }
            Item existedItem = await db.Items.FindAsync(id);
            if (item.PhotoId == null) item.PhotoId = existedItem.PhotoId;
            db.Entry(existedItem).CurrentValues.SetValues(item);
            await db.SaveChangesAsync();
            return Ok(db.Items.Where(x => x.Id == item.Id).Select(x => new
            {
                id = x.Id,
                value = x.Value,
                photo = x.Photo.Target.Path,
                code = x.Code,
                description = x.Description,
                deletable = (db.Inventories.Where(y => y.ItemId == x.Id).Count() == 0),
                unit =new { no=x.Unit.No},
                unitId=x.UnitId,
                unitTypeId = x.UnitTypeId
            }).FirstOrDefault());
        }

        [Route("api/Items/Status/{id}")]
        public async Task<IHttpActionResult> PutItemStatus(Guid id, Item item)
        {
            item = await db.Items.FindAsync(id);
            Item existedItem = await db.Items.FindAsync(id);
            if (item.Status.Code != "statusD") item.StatusId = (await db.Options.FirstOrDefaultAsync(x => x.Code == "statusD")).Id;
            else item.StatusId = (await db.Options.FirstOrDefaultAsync(x => x.Code == "statusR")).Id;
            db.Entry(existedItem).CurrentValues.SetValues(item);
            await db.SaveChangesAsync();
            return Ok(existedItem);
        }

        [Route("api/Items/Save/{id}")]
        public async Task<IHttpActionResult> PutItemSave(Guid id, ItemSave itemSave)
        {
            HashSet<Category> categories = new HashSet<Category>();
            if (itemSave.Item.Description != null)
            {

                string[] categoryValuesArray = itemSave.Item.Description.Split('#');
                foreach (var categoryValues in categoryValuesArray)
                {
                    string categoryValue = categoryValues;
                    if (categoryValue != "" && categories.Count(x => x.Value == categoryValue) == 0)
                    {
                        Category category = await db.Categories.FirstOrDefaultAsync(x => x.Value == categoryValues.Trim());
                        if (category == null)
                        {
                            category = new Category { Value = categoryValues.Trim() };
                            db.Categories.Add(category);
                            await db.SaveChangesAsync();

                        }
                        categories.Add(category);


                    }
                }
            }

            foreach (Category category in categories)
            {
                ItemCategory itemCategory = await db.ItemCategories.FirstOrDefaultAsync(x => x.OwnerId == id && x.TargetId == category.Id);
                if (itemCategory == null)
                {
                    db.ItemCategories.Add(new ItemCategory
                    {
                        StartDate = DateTime.Now,
                        OwnerId = itemSave.Item.Id,
                        TargetId = category.Id

                    });
                    await db.SaveChangesAsync();
                }
            }
            foreach (ItemCategory itemCategory in db.ItemCategories.Where(x => x.OwnerId == id).ToArray())
            {
                Category category = categories.FirstOrDefault(x => x.Id == itemCategory.TargetId);
                if (category == null)
                {
                    db.ItemCategories.Remove(itemCategory);/*
                    itemCategory.EndDate = DateTime.Now;
                    itemCategory.ArchivedDate = itemCategory.EndDate;
                    ItemCategory existedItemCategory = await db.ItemCategories.FindAsync(itemCategory.Id);
                    db.Entry(existedItemCategory).CurrentValues.SetValues(itemCategory);*/
                }
            }
            Item existedItem = await db.Items.FindAsync(id);
            db.Entry(existedItem).CurrentValues.SetValues(itemSave.Item);
            await db.SaveChangesAsync();
            return Ok(existedItem);
        }

        public async Task<IHttpActionResult> PostItem(Item item)
        {
            item.Unit = null;
            if (item.Value == "")
            {
                string userId = HttpContext.Current.User.Identity.Name;
                Guid inventoryId = (await db.Inventories.FirstOrDefaultAsync(x => x.UserId == userId)).Id;
                Item existedItem = db.Items.FirstOrDefault(x => x.Value.ToString() == "" && x.CreatedById == inventoryId);
                if (existedItem != null) return Ok(db.Items.Where(x => x.Id == existedItem.Id).Select(x => new
                {
                    id = x.Id,
                    value = x.Value,
                    photo = x.Photo.Target.Path,
                    code = x.Code,
                    description = x.Description,
                    deletable = (db.Inventories.Where(y => y.ItemId == x.Id).Count() == 0)
                }).FirstOrDefault());
            }
           
                //  item.StatusId = new Guid("005617B3-D283-461C-ABEF-5C0C16C780D0");
                db.Items.Add(item);
                await db.SaveChangesAsync();
                await db.Entry(item).GetDatabaseValuesAsync();
           
            
            HashSet<string> categoryValues = new HashSet<string>();
            if (item.Description.ToString() != "")
            {
                foreach (string _categoryValue in item.Description.Split('#'))
                {
                    string categoryValue = _categoryValue.Trim();
                    if (categoryValue != "")
                    {
                        ItemCategory itemCategory = new ItemCategory
                        {
                            StartDate = DateTime.Now,
                            OwnerId = item.Id
                        };
                        Category category = await db.Categories.FirstOrDefaultAsync(x => x.Value == categoryValue);
                        if (category != null)
                        {
                            itemCategory.TargetId = category.Id;
                        }
                        else
                        {
                            itemCategory.Target = new Category
                            {
                                Value = categoryValue
                            };                 
                        }
                        db.ItemCategories.Add(itemCategory);
                    }
                }
                await db.SaveChangesAsync();
            }
            return Ok(db.Items.Where(x => x.Id == item.Id).Select(x => new
            {
                id = x.Id,
                value = x.Value,
                photo = x.Photo.Target.Path,
                code = x.Code,
                description = x.Description,
                deletable = (db.Inventories.Where(y => y.ItemId == x.Id).Count() == 0)
            }).FirstOrDefault());
        }

        public async Task<IHttpActionResult> DeleteItem(Guid id)
        {
            Item item = await db.Items.FindAsync(id);
            if (item == null) return NotFound();
            Item existedItem = await db.Items.FindAsync(item.Id);
            db.Entry(existedItem).CurrentValues.SetValues(item);
            await db.SaveChangesAsync();
            foreach (ItemCategory itemCategory in db.ItemCategories.Where(x => x.OwnerId == id).ToList())
            {
                db.ItemCategories.Remove(itemCategory);
            }
            foreach (Entities.Attribute attribute in db.ItemAttributes.Where(x => x.OwnerId == id).Select(x => x.Target).ToList())
            {
                db.Attributes.Remove(attribute);
            }
            db.Items.Remove(item);
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