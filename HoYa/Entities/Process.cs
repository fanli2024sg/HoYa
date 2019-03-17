using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{
    public class WorkFlow : NodeDefinition<WorkFlow, WorkFlowChange>
    {
       
    }
    public class WorkFlowChange : Change<WorkFlow>
    {
    }

    public class Step : Definition<StepChange>
    {
    }
    public class StepChange : Change<Step>
    {
    }

    public class Mission : Instance<WorkFlowChange>
    {

    }

    public class Process : Instance<WorkFlow>
    {
        [MaxLength(256)] public string No { get; set; }

        public virtual Guid? StepId { get; set; }
        [ForeignKey("StepId")]
        public virtual Step Step { get; set; }

        public DateTime? OverTime { get; set; }
    }

  

    public abstract class General : Entity
    {
        public virtual Guid? ProcessingId { get; set; }
        [ForeignKey("ProcessingId")]
        public virtual Process Processing { get; set; }
    }




}