using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HoYa.Entities;

namespace HoYa.Models
{ 
    public class CategorySelect
    {
        public Guid Id { get; set; } 
        public string Value { get; set; }
        public decimal? ItemQuantity { get; set; }
    } 

    public class CategoryGrid
    {
        public Category Category { get; set; }

        public int ItemQuantity { get; set; }
    } 
}