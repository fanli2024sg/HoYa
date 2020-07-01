using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HoYa.Entities;

namespace HoYa.Models
{
    public class InventoryWithAttributes
    {
        public Guid Id { get; set; }
        public string No { get; set; }
        public Guid? StatusId { get; set; }
        public Guid? ItemId { get; set; }
        public float? Value { get; set; }
        public Position Position { get; set; }
        public Guid? PositionId { get; set; }
        public ICollection<InventoryAttribute> Attributes { get; set; } = new HashSet<InventoryAttribute>();
    }

    public class InventoryPickup
    {
        public Guid Id { get; set; }
        public string Memo { get; set; }
        public Guid ItemId { get; set; }
        public Guid TargetId { get; set; }
        public Guid WorkOrderId { get; set; }
        public ICollection<Segmentation> Segmentations { get; set; } = new HashSet<Segmentation>();
    }

    public class InventoryStartup
    {
        public float UpValue { get; set; }
        public string Memo { get; set; }
        public Guid ItemId { get; set; }
        public Guid TargetId { get; set; }
        public Guid WorkOrderId { get; set; }
        public ICollection<Defect> Defects { get; set; } = new HashSet<Defect>();
    }
    public class InventoryInspection
    {
        public float UpValue { get; set; }
        public string Memo { get; set; }
        public Guid ItemId { get; set; }
        public Guid TargetId { get; set; }
        public Guid WorkOrderId { get; set; }
        public ICollection<Defect> Defects { get; set; } = new HashSet<Defect>();
    }

    public class InventoryStation
    {
        public string Action { get; set; }
        public float UsedPower { get; set; }
        public float UpValue { get; set; }
        public string Memo { get; set; }
        public Guid TargetId { get; set; }
        public Guid WorkOrderId { get; set; }
        public ICollection<Reason> Reasons { get; set; } = new HashSet<Reason>();
    }

    public class Defect
    {
        public string AttributeValue { get; set; }
        public float Value { get; set; } 
    }

    public class Reason
    {
        public string Value { get; set; }
    }


    public class ItemUpload
    {  
        public string Code { get; set; }
        public string Value { get; set; }      
        public string Description { get; set; }
    }

    public class InventoryAttributeList
    {
        public string Attribute { get; set; }
        public string Value { get; set; }
    }
    public class InventorySelect
    {
        public Guid Id { get; set; }
        public string No { get; set; }
        public string SubValue { get; set; }
    }

    public class InventoryBase
    {
        public Guid Id { get; set; }
        public string No { get; set; }
        public string Sort { get; set; }
    }

    public class InventoryPosition
    {
        public Guid? Id { get; set; }
        public string No { get; set; }
    }

    public class CategoryList
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
    }

    public class AttributeList
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
    }

    public class InventoryGrid
    {
        public Inventory Inventory { get; set; }
        public int InventoryQuantity { get; set; }
        public int PhotoQuantity { get; set; }
        public float? SegmentationsQuantitySum { get; set; }
    }

    public class InventoryList
    {
        public Guid? Id { get; set; }
        public string No { get; set; }
        public string Sort { get; set; }
        public float? Value { get; set; }
        public string _photo { get; set; }
        public Guid? ItemId { get; set; }
        public string _item { get; set; }
        public Guid? PositionId { get; set; }
        public string _position { get; set; }
    }
    public class InventoryPrint
    {
        public Guid? Id { get; set; }
        public string No { get; set; }
    }

    public class InventoryPutdown
    {
        public Guid? Id { get; set; }
        public string No { get; set; }
        public string Unit { get; set; }
        public float? Value { get; set; }
        public Guid? RecipeId { get; set; }
        public string RecipeValue { get; set; }
        public Guid? ItemId { get; set; }
        public string ItemValue { get; set; }
        public string File { get; set; }
    }

    public class InventoryAccordion
    {
        public Guid? Id { get; set; }
        public string No { get; set; }
        public string RecipeValue { get; set; }
        public string RecipeId { get; set; }
        public string PositionId { get; set; }
        public string PositionNo { get; set; }
        public string PositionRecipeValue { get; set; }
        public string PositionRecipeId { get; set; }
        public string ParentPositionId { get; set; }
        public string ParentPositionNo { get; set; }
        public string ParentPositionRecipeValue { get; set; }
        public string ParentPositionRecipeId { get; set; }
    }
    public class InventorySave
    {
        public Inventory Inventory { get; set; }
        public ICollection<FolderFile> Photos { get; set; } = new HashSet<FolderFile>();
        public ICollection<InventoryAttribute> Details { get; set; } = new HashSet<InventoryAttribute>();
    }

    public class PositionAction
    {
        public string Action { get; set; }
        public Inventory Owner { get; set; }
        public Inventory Target { get; set; }
    }

    public class InventoryPutDown
    {
        public string Action { get; set; }
        public Inventory Target { get; set; }
    }

   
}