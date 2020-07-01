using System;
using System.Collections.Generic; 
using HoYa.Entities;

namespace HoYa.Models
{
    public class RecipeSave
    {
        public Recipe Recipe { get; set; }
        public ICollection<FolderFile> Photos { get; set; } = new HashSet<FolderFile>();
    }
    public class RecipeList
    {
        public Guid Id { get; set; }
        public string No { get; set; }
        public string _photo { get; set; }
        public bool _deletable { get; set; }
        public ICollection<InventoryRelationship> _relationships { get; set; } =new  HashSet<InventoryRelationship>();
    }

    public class InventoryRelationship
    {
        public Guid? AttributeId { get; set; }
        public Entities.Attribute Attribute { get; set; }

        public ICollection<Inventory> Targets { get; set; } = new HashSet<Inventory>();
    }


    


    public class RecipeDetail
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string Photo { get; set; }
        public string Status { get; set; }
        public bool Deletable { get; set; }
    }
    public class RecipeSelect
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
    } 
    public class RecipeGrid
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public string Photo { get; set; }
        public Guid? ItemId { get; set; }
    }
    public class RecipeAccordion
    {
        public Guid? Id { get; set; }
        public string Value { get; set; }
        public string ItemId { get; set; }
        public string ItemValue { get; set; }
    }
}