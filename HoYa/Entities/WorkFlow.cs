using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{
    public class WorkFlowDefinition : Definition
    {
    }


    public class WorkFlow : Branch<WorkFlowDefinition>
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public virtual Guid? StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual Option Status { get; set; }
    }

    public class WorkFlowChange : Change<WorkFlow>
    {
    }

    public class Step : SimpleDetail<WorkFlow>
    {

    }

    public class StepGroup : Relation<Step, Group>
    {
    }

    public class Activity : Relation<Process,Step>
    {
        public virtual Guid? ParticipantId { get; set; }
        [ForeignKey("ParticipantId")]
        public virtual Profile Participant { get; set; }
    }

    public class Process : NodeInstance<WorkFlow, WorkFlowChange,Process>
    {
    }


}