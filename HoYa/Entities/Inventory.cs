using Newtonsoft.Json;
using System;
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

        public float? Value { get; set; }

        public virtual Guid? PositionId { get; set; }
        [ForeignKey("PositionId")] 
        public virtual Position Position { get; set; }

        public virtual Guid? PhotoId { get; set; }
        [ForeignKey("PhotoId")]
        public virtual FolderFile Photo { get; set; }
    }

    public class InventoryAttribute : Relation<Inventory, Attribute>
    {
        public string Value { get; set; }
    }


    public class Position : Relation<Inventory, Inventory>//搭乘某載具有起訖時間 Target載具 Target is null as 絕對3D位置
    {
        public virtual Guid? PreOwnerId { get; set; }
        [ForeignKey("PreOwnerId")]
        public virtual Inventory PreOwner { get; set; }
    }

    public class Segmentation : Relation<Inventory, Inventory>//產生另一批Inventory=> Target Inventory 原Inventory減量
    {
        public float? Quantity { get; set; }
    }

    public class Relationship : Relation<Inventory, Inventory>
    { 
        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }
    }
}