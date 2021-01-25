using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{ 
    public class Item : Definition
    {
        public string Description { get; set; }
        public virtual Guid? PhotoId { get; set; }
        public virtual FolderFile Photo { get; set; }

        public virtual Guid? RecipeId { get; set; }
        [ForeignKey("RecipeId")]
        public virtual Recipe Recipe { get; set; }

        public virtual Guid? DefaultId { get; set; }
        [ForeignKey("DefaultId")]
        public virtual Inventory Default { get; set; }
        public virtual Guid? UnitId { get; set; }
        [ForeignKey("UnitId")]
        public virtual Inventory Unit { get; set; }
        public virtual Guid? UnitTypeId { get; set; }
        [ForeignKey("UnitTypeId")]
        public virtual Inventory UnitType { get; set; }

    }

    public class ItemAttribute : Relation<Item,Attribute>
    {
    } 

    public class ItemCategory : Relation<Item, Category>
    {
    }

    public class ItemGroup : Relation<Item, Inventory>
    {
    }
}