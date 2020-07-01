using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HoYa.Entities;

namespace HoYa.Models
{
    public class Query<O>
    {
        public int PaginatorLength { get; set; }
        public ICollection<O> Data { get; set; } = new HashSet<O>();
    }
}