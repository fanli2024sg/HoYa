using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{
    public class WorkFlow : Definition
    {
    }


    public class Step : SimpleDetail<WorkFlow>
    {

    }

    public class Mission : Instance<Step>
    {
        public virtual Guid? ProcessId { get; set; }
        [ForeignKey("ProcessId")]
        public virtual Process Process { get; set; }

        public virtual Guid? GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }

        public virtual Guid? ProfileId { get; set; }
        [ForeignKey("ProfileId")]
        public virtual Profile Profile { get; set; }
    }

    public class Process : NodeGeneral<Process, WorkFlow>
    {
    }
}