using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{
    public class WorkFlow : Definition
    {
        public virtual Guid? FirstId { get; set; }
        [ForeignKey("FirstId")]
        [JsonIgnore]
        public virtual Step First { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public virtual Guid? StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual Option Status { get; set; }
        public virtual Guid? FinalId { get; set; }
        [ForeignKey("FinalId")]
        [JsonIgnore]
        public virtual Step Final { get; set; }
    }

    public class Step : Base
    {
        public virtual Guid? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public virtual WorkFlow Owner { get; set; }
        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }
        public decimal? Sort { get; set; }
        public string Value { get; set; }
        public string View { get; set; }
        public decimal? SubmitRatio { get; set; }
        public string Component { get; set; }
    }

    public class Redirect : Relation<Step, Step>
    {
        public decimal? TrueRatio { get; set; }//(=100)=AND (>0)=OR
    }

    public class RedirectCondition : Relation<Redirect, Condition>
    {
        public decimal? Sort { get; set; }
    }

    public class Condition : Definition
    {
    }

    public class StepGroup : Relation<Step, Inventory>
    {
        public float? Sort { get; set; }
    }

    public class StepRelationship : Relation<Step, Option>//這裡的Option是Of的關係 例如主管 或 代理人
    {
        public decimal? Sort { get; set; }
        public virtual Guid? OfId { get; set; }
        [ForeignKey("OfId")]
        public virtual Step Of { get; set; }//開單者 或 前一關卡者
    }

    public class Activity : Relation<Process, Step>
    {
        public virtual Guid? PreviousId { get; set; }
        [ForeignKey("PreviousId")]
        public virtual Activity Previous { get; set; }
        public virtual Guid? NextId { get; set; }
        [ForeignKey("NextId")]
        [JsonIgnore]
        public virtual Activity Next { get; set; }
        public virtual Guid? ArchivedParticipateId { get; set; }
        [ForeignKey("ArchivedParticipateId")]
        [JsonIgnore]
        public virtual Participate ArchivedParticipate { get; set; }
    }

    public class Participate :Base
    {
        public virtual Guid? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        [JsonIgnore]
        public virtual Activity Owner { get; set; }
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? ArchivedDate { get; set; }

        public virtual Guid? ArchivedById { get; set; }
        [ForeignKey("ArchivedById")]
        [JsonIgnore]
        public virtual Inventory ArchivedBy { get; set; }

        public virtual Guid? ParticipantId { get; set; }
        [ForeignKey("ParticipantId")]
        public virtual Inventory Participant { get; set; }

        public virtual Guid? RedirectId { get; set; }
        [ForeignKey("RedirectId")]
        public virtual Redirect Redirect { get; set; }

        public DateTime? ReadedDate { get; set; }
        public Participate()
        {
            StartDate = DateTime.Now;
        }
    }

    public class Judgment :Base
    {
        public virtual Guid? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        [JsonIgnore]
        public virtual Participate Owner { get; set; }
        public virtual Guid? ConditionId { get; set; }
        [ForeignKey("ConditionId")]
        public virtual Condition Condition { get; set; }
    }

    public class Process : Instance
    {
        public virtual Guid? WorkFlowId { get; set; }
        [ForeignKey("WorkFlowId")]
        public virtual WorkFlow WorkFlow { get; set; }
        public virtual Guid? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Process Parent { get; set; }
        public int? ReEdit { get; set; }
        public virtual Guid? PreviousId { get; set; }
        [ForeignKey("PreviousId")]
        public virtual Activity Previous { get; set; }
        public virtual Guid? CurrentId { get; set; }
        [ForeignKey("CurrentId")]
        public virtual Activity Current { get; set; }
    }
}