using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HoYa.Entities
{
    //公共授權 type=ehs
    //門禁授權 type=gate
    //部門建立 type=hr
    public class Group : TypeSimple
    {

        public virtual Guid? ProfileId { get; set; }
        [ForeignKey("ProfileId")]
        public virtual Profile Profile { get; set; }
        public string Code { get; set; }
        public virtual Guid? StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual Option Status { get; set; }

        public virtual Guid? ChangeId { get; set; }
        [ForeignKey("ChangeId")]
        public virtual GroupChange Change { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    
    //門禁授權用
    public class GroupChangeGeneral : General
    {
        public string PersonIds { get; set; }
    }
    //公共授權 type=ehs
    //門禁授權 type=gate
    //部門建立 type=hr
    public class GroupChange : Change<Group>
    {
        public Guid? TypeId { get; set; }
        public string GateIds { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }



    //可用功能
    public class FunctionGroup : Association<Function, Group>
    {
    }

    /// <summary>
    /// 可用功能
    /// </summary>
    public class Function : Simple
    {
    }

    //群組規則
    public class Rule : SimpleDetail<Group>
    {
    }

    //選單
    public class Menu : SimpleNode<Menu>
    {
        public virtual Guid? FunctionId { get; set; }
        [ForeignKey("FunctionId")]
        public virtual Function Function { get; set; }
    }
}