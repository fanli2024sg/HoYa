using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{
    public abstract class Base
    { 
        public virtual Guid Id { get; set; }

        public int Version { get; set; }

        public virtual Guid? CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        [JsonIgnore]
        public virtual Inventory CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Base()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
        }
    }

    public class Definition : Base
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public virtual Guid? StatusId { get; set; }
        [ForeignKey("StatusId")]
        [JsonIgnore]
        public virtual Option Status { get; set; }
    }
    
    public abstract class Relation<O, T> : Base
    {
        public int TargetVersion { get; set; }
        public virtual Guid? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        [JsonIgnore]
        public virtual O Owner { get; set; }
        public virtual Guid? TargetId { get; set; }
        [ForeignKey("TargetId")]
        public virtual T Target { get; set; }

        public DateTime? StartDate { get; set; }//前端填寫

        public DateTime? EndDate { get; set; }//前端填寫

        public DateTime? ArchivedDate { get; set; }//後端填寫

       public virtual Guid? ArchivedById { get; set; }//後端填寫
        [ForeignKey("ArchivedById")]
        [JsonIgnore]
        public virtual Inventory ArchivedBy { get; set; }//後端填寫
        public virtual Guid? CreatedParticipateId { get; set; }//後端填寫
        [ForeignKey("CreatedParticipateId")]
        [JsonIgnore]
        public virtual Participate CreatedParticipate { get; set; }//後端填寫
        public virtual Guid? ArchivedParticipateId { get; set; }//後端填寫
        [ForeignKey("ArchivedParticipateId")]
        [JsonIgnore]
        public virtual Participate ArchivedParticipate { get; set; }//後端填寫
        public Relation()
        {
        }
    }

 

    public class Instance : Base
    {
        
        public string No { get; set; }

        public virtual Guid? StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual Option Status { get; set; }
    }

    public class Option : Definition
    {
        public virtual Guid? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Option Parent { get; set; }
    } 
    public class FolderFile : Base
    {
        public string FolderType { get; set; } 
        public virtual Guid? FolderId { get; set; }

        public virtual Guid? TargetId { get; set; }
        [ForeignKey("TargetId")]
        public virtual File Target { get; set; }

        public DateTime? StartDate { get; set; }//前端填寫

        public DateTime? EndDate { get; set; }//前端填寫

        public DateTime? ArchivedDate { get; set; }//後端填寫

        public virtual Guid? ArchivedById { get; set; }//後端填寫
        [ForeignKey("ArchivedById")]
        [JsonIgnore]
        public virtual Inventory ArchivedBy { get; set; }//後端填寫
        public virtual Guid? CreatedParticipateId { get; set; }//後端填寫
        [ForeignKey("CreatedParticipateId")]
        [JsonIgnore]
        public virtual Participate CreatedParticipate { get; set; }//後端填寫
        public virtual Guid? ArchivedParticipateId { get; set; }//後端填寫
        [ForeignKey("ArchivedParticipateId")]
        [JsonIgnore]
        public virtual Participate ArchivedParticipate { get; set; }//後端填寫
        public FolderFile()
        {
        }
    }
    public class File : Definition
    {
        public string Path { get; set; }
        public string Url { get; set; }
    }
}
