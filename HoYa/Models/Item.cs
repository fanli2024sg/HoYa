using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HoYa.Entities;

namespace HoYa.Models
{
    public class ItemAttributesList
    { 

    }
    public class ItemSave
    {
        public Item Item { get; set; }
        public ICollection<string> CategoryValues { get; set; } = new HashSet<string>();

        public ICollection<FolderFile> Photos { get; set; } = new HashSet<FolderFile>();
    } 
    public class ItemSelect
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string Photo { get; set; }
    } 
    public class ItemList
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public string _photo { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool? _deletable { get; set; }
    }
    public class ItemDetail
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public string Photo { get; set; }
        public string Status { get; set; }
        public bool? Deletable { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    } 
    public class ItemGrid
    {
        public Item Item { get; set; }
        public int RecipeDraftQuantity { get; set; }
        public int RecipeQuantity { get; set; }

        public int InventoryQuantity { get; set; }

        public int PhotoQuantity { get; set; }
    }
     
}