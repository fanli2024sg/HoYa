using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HoYa.Entities;

namespace HoYa.Models
{
    public class FileSave
    { 
        public ICollection<FolderFile> Photos { get; set; } = new HashSet<FolderFile>();
    }

    public class OptionSelect
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
    }

    public class Accordion
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
    }
    public class Grid
    {
        public Guid? Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Key { get; set; }
        public string File { get; set; }
    }
}