using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{
    //實例化Material
    public class Inventory : Instance
    {
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AspNetUser AspNetUser { get; set; }

        public virtual Guid? ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }

        public decimal? Value { get; set; }

        public virtual Guid? PositionId { get; set; }
        [ForeignKey("PositionId")] 
        public virtual Position Position { get; set; }
        /*
        public DateTime? ConsumedDate { get; set; }//後端填寫

        public virtual Guid? ConsumedById { get; set; }//後端填寫
        [ForeignKey("ConsumedById")]
        [JsonIgnore]
        public virtual Inventory ConsumedBy { get; set; }//後端填寫

        public DateTime? ProducedDate { get; set; }//後端填寫

        public virtual Guid? ProducedById { get; set; }//後端填寫
        [ForeignKey("ProducedById")]
        [JsonIgnore]
        public virtual Inventory ProducedBy { get; set; }//後端填寫
        */
        public virtual Guid? PhotoId { get; set; }
        [ForeignKey("PhotoId")]
        public virtual FolderFile Photo { get; set; } 
        public bool? Archived { get; set; }
        public Inventory()
        {
            this.Value = 1;
        }
        public Inventory(string itemId)
        {
            this.Value = 1;
            this.ItemId = Guid.Parse(itemId);
        } 
        public void Deconstruct(out Guid? itemId, out Guid? positionTargetId)
        {
            itemId = ItemId;
            positionTargetId = Position.TargetId;
        }
    }

    public class InventoryAttribute : Relation<Inventory, Attribute>
    {
        public string Value { get; set; }
        public InventoryAttribute()
        {
        }
        public InventoryAttribute(string targetId)
        {
            this.TargetId = Guid.Parse(targetId);
        }
    }
    public class InventoryCategory : Relation<Inventory, Category>
    {

        public InventoryCategory()
        {
        }
        public InventoryCategory(string targetId)
        {
            this.TargetId = Guid.Parse(targetId);
        }
    }

    public class Position : Relation<Inventory, Inventory>//搭乘某載具有起訖時間 Target載具 Target is null as 絕對3D位置
    {
        public virtual Guid? PreOwnerId { get; set; }
        [ForeignKey("PreOwnerId")]
        public virtual Inventory PreOwner { get; set; }
    }

    public class Segmentation : Relation<Inventory, Inventory>//產生另一批Inventory=> Target Inventory 原Inventory減量
    {
        public decimal? Quantity { get; set; }
        public Segmentation()
        {
            this.Quantity = 0;
        }
        public Segmentation(decimal? quantity)
        {
            this.Quantity = quantity;
        }
    }

    public class Relationship : Relation<Inventory, Inventory>
    { 
        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }
    }

    public class ProfileGroup : Relation<Inventory, Inventory>
    {
    }
    
    public class InventoryGroup : Relation<Inventory, Inventory>
    {
    }

    public class Exchange : Relation<Inventory, Inventory>
    {
        public decimal? Value { get; set; }
    }
}