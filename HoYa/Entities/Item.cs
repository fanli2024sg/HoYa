using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{ 
    public class Item : Definition
    {
        public string Description { get; set; }
        public virtual Guid? PhotoId { get; set; }
        public virtual FolderFile Photo { get; set; }
    }

    public class ItemAttribute : Relation<Item,Attribute>
    {
    } 

    public class ItemCategory : Relation<Item, Category>
    {
    }
}