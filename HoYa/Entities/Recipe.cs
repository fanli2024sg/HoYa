using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{       
    public class Recipe : Definition
    {
        public virtual Guid? PhotoId { get; set; }
        [ForeignKey("PhotoId")]
        public virtual FolderFile Photo { get; set; }
    }

    public class ProducedBy : Relation<Item, Recipe>
    {
    }

    public class RequiredBy : Relation<Recipe, Item>
    {
    }


    
}