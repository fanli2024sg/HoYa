using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HoYa.Entities;

namespace HoYa.Models
{
    public class AttributeWithValue
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public Guid? TargetId { get; set; }
        public string TargetValue { get; set; }
        public int Level { get; set; }
        public string ValueType { get; set; }
        public string ItemIds { get; set; }
        public string InventoryIds { get; set; }
        public string CategoryIds { get; set; }
    }
    public class AttributeWithItem
    {  
        public string Value { get; set; }      
        public string Type { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid? ItemId { get; set; }
        public bool Must { get; set; }
    }

    public class AttributeWithCate
    {
        public string Value { get; set; }
        public string Type { get; set; }
        public string CategoryValues { get; set; }
        public bool Must { get; set; } 
    }
}