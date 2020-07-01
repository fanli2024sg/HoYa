using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HoYa.Entities;

namespace HoYa.Models
{
    public class PersonOption
    {
        public Guid Id { get; set; }
        public Guid? InventoryId { get; set; }
        public string Value { get; set; }
        public string InventoryCardTargetNo { get; set; }
        public string employerNo { get; set; }
        public string Name { get; internal set; }
        public string ExperienceEmployerShortName { get; set; }
        public DateTime? InventoryCardEndDate { get; set; }
        public string PhoneValue { get; set; }
    }

    public class PostPersonOption
    {
        public string anyLike { get; set; } = "";
        public Guid? typeId { get; set; }
        public Guid? experienceEmployerId { get; set; }
        public int? pageSize { get; set; } = 10;

        public ICollection<Guid> excludePersonIds { get; set; } = new HashSet<Guid>();
    }


    

    public class DocumentOption
    {
        public Guid Id { get; set; }
        public Guid? OwnerId { get; set; }
        public string No { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
    }

    public class ExperienceOption
    {
        public Guid Id { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid? EmployerId { get; set; }
        public string EmployerShortName { get; set; }
        public string EmployerCode { get; set; }
    }
}