using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoYa.Entities
{
    public class SearchResult
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public string SubTitle { get; set; }
        public string PhotoPath { get; set; }
        public string NavigateUrl { get; set; }
    }

    public class SearchCondition
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string Desc { get; set; }
    }
}