using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{       
    public class Category : Definition
    {
        public string Color { get; set; }
    }

    public class CategoryAttribute : Relation<Category, Attribute>
    {
        public string Type { get; set; }

        public bool Must { get; set; }
    }

    
}