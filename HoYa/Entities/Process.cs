using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{
    public class Process : TypeSimpleDefinition<ProcessChange>
    {
       
    }

    public class ProcessChange : Change<Process>
    { 
    } 
    public class Mission : Detail<ProcessChange>
    {

    }

    public class Processing : TypeNodeInstance<Processing, Process, ProcessChange>
    {
        [MaxLength(256)] public string No { get; set; }

        public virtual Guid? StepId { get; set; }
        [ForeignKey("StepId")]
        public virtual Step Step { get; set; }

        public DateTime? OverTime { get; set; }
    }

    public class Step : TypeSimpleDetail<Processing>//TypeId=一般作業 特殊作業 緊急臨時特殊作業 Type.ParentId= 訪客 上課 施工
    {
        public virtual Guid? ProcessingId { get; set; }
        [ForeignKey("ProcessingId")]
        public virtual Processing Processing { get; set; }
        public string Comment { get; set; }
        public virtual Guid? ActionId { get; set; }
        [ForeignKey("ActionId")]
        public virtual Option Action { get; set; }
        public DateTime? ActionDate { get; set; }

        public DateTime? OverTime { get; set; }
    }

    public abstract class Change<E> : Entity
    {
        public virtual Guid? ProcessingId { get; set; }
        [ForeignKey("ProcessingId")]
        public virtual Processing Processing { get; set; }

        public virtual Guid? EntityId { get; set; }
        [ForeignKey("EntityId")]
        [JsonIgnore]
        public virtual E Entity { get; set; }
    }

    public abstract class General : Entity
    {
        public virtual Guid? ProcessingId { get; set; }
        [ForeignKey("ProcessingId")]
        public virtual Processing Processing { get; set; }
    }




}