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
    public class Group : TypeDefinition
    { 
    }
    


    //可用功能
    public class FunctionGroup : Relation<Function, Group>
    {
    }

    /// <summary>
    /// 可用功能
    /// </summary>
    public class Function : Base
    {
    }

    //群組規則
    public class Rule : Detail<Group>
    {
    }

    //選單
    public class Menu : Node<Menu>
    {
        public string Value { get; set; }
        public string Code { get; set; }
        public virtual Guid? FunctionId { get; set; }
        [ForeignKey("FunctionId")]
        public virtual Function Function { get; set; }
    }
}