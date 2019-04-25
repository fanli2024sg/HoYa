using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{
    public class Item : RealTypeDefinition
    {
    }

    public class Recipe : Branch<Item>
    {
    }

    public class RecipeChange : Change<Recipe>
    {
    }

    public class Procedure : Detail<Recipe>
    {
        public virtual Guid? IngredientId { get; set; }
        [ForeignKey("IngredientId")]
        public virtual Recipe Ingredient { get; set; }
        public float? Quantity { get; set; }
    }

    public class ItemCategory : Relation<Item, Category>
    {
    }
}