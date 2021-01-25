using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{
    public class Recipe : Instance
    {
        public virtual Guid? ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }
        public virtual Guid? PhotoId { get; set; }
        [ForeignKey("PhotoId")]
        public virtual FolderFile Photo { get; set; }
        public Recipe()
        {
        }
        public Recipe(string recipeId)
        {
            this.Id = Guid.Parse(recipeId);
        }
    }

    public class Input : Detail<Recipe>
    { 
        public virtual Guid? ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }
        public decimal? Value { get; set; }
        public virtual Guid? UnitId { get; set; }
        [ForeignKey("UnitId")]
        public virtual Inventory Unit { get; set; }
        public Input()
        {
        }
        public Input(string itemId)
        {
            this.ItemId = Guid.Parse(itemId);
        }
    }

    public class Output : Instance
    {
        public virtual Guid? ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }
        public decimal? Value { get; set; }
        public virtual Guid? UnitId { get; set; }
        [ForeignKey("UnitId")]
        public virtual Inventory Unit { get; set; }
        public Output()
        {
        }
        public Output(string itemId)
        {
            this.ItemId = Guid.Parse(itemId);
        }
    }

    public class RecipeGroup : Relation<Recipe, Inventory>
    {
    }
}