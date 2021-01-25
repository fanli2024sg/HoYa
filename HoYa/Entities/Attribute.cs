using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{
    public class Attribute : Definition
    {
        public string Value { get; set; }
        public int Level { get; set; }
        public string ValueType { get; set; }
        public int ValueNumber { get; set; }
        public string ItemIds { get; set; }
        public string InventoryIds { get; set; }
        public string CategoryIds { get; set; }
    }

    public class AttributeGroup : Relation<Attribute, Inventory>
    {
    }
}