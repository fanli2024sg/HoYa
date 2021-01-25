using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{
    public class WorkPlan : Instance
    {
        public virtual Guid? ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }
        public virtual Guid? WorkPlanRecordId { get; set; }
        [ForeignKey("WorkPlanRecordId")]
        public virtual WorkPlanRecord WorkPlanRecord { get; set; }


        public DateTime? StartDate { get; set; }//後端填寫

        public virtual Guid? StartId { get; set; }//後端填寫
        [ForeignKey("StartId")]
        [JsonIgnore]
        public virtual WorkEvent Start { get; set; }//後端填寫


        public DateTime? EndDate { get; set; }//後端填寫

        public virtual Guid? EndId { get; set; }//後端填寫
        [ForeignKey("EndId")]
        [JsonIgnore]
        public virtual WorkEvent End { get; set; }//後端填寫


        public WorkPlan()
        {
        } 
    }

    public class WorkPlanRecord : Detail<WorkPlan>
    {
        public decimal? Value { get; set; }
        public virtual Guid? UnitId { get; set; }
        [ForeignKey("UnitId")]
        public virtual Inventory Unit { get; set; }
        public int Sort { get; set; }
        public WorkPlanRecord()
        {
        }
    }

    public class WorkOrder : Detail<WorkPlan>
    {
        public virtual Guid? ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }

        public DateTime? StartDate { get; set; }//後端填寫

        public virtual Guid? StartId { get; set; }//後端填寫
        [ForeignKey("StartId")]
        [JsonIgnore]
        public virtual WorkEvent Start { get; set; }//後端填寫


        public DateTime? EndDate { get; set; }//後端填寫

        public virtual Guid? EndId { get; set; }//後端填寫
        [ForeignKey("EndId")]
        [JsonIgnore]
        public virtual WorkEvent End { get; set; }//後端填寫  

        public bool? Archived { get; set; }
        public decimal? Value { get; set; }
        public int Sort { get; set; } 
        public WorkOrder(string ownerId)
        {
            this.OwnerId = Guid.Parse(ownerId);
        }
    }

    public class WorkEvent : Detail<WorkOrder>
    {
        public int Sort { get; set; }

        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }

        public WorkEvent(string ownerId, string typeId)
        {
            this.OwnerId = Guid.Parse(ownerId);
            this.TypeId = Guid.Parse(typeId);
        } 
    }

    public class WorkEventInventory : Relation<WorkEvent,Inventory>//Position在Station上
    {
        public int Sort { get; set; }

        public virtual Guid? TypeId { get; set; }//Input(Position=Station), Output(Position=Station), Pickup(Position=Person)
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }

        public WorkEventInventory(string ownerId, string targetId)
        {
            this.OwnerId = Guid.Parse(ownerId);
            this.TargetId = Guid.Parse(targetId);
        }
    }
     

    public class StationRecord : Relation<Inventory,WorkEvent>
    {
        public int Sort { get; set; }
        public StationRecord(string ownerId, string targetId)
        {
            this.OwnerId = Guid.Parse(ownerId);
            this.TargetId = Guid.Parse(targetId);
        }
    } 
}