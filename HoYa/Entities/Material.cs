using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{
    //設備
    //物料
    public class Material : TypeSimple
    {
        public string Code { get; set; }
        public virtual Guid? UnitId { get; set; }
        [ForeignKey("UnitId")]
        public virtual Option Unit { get; set; }

        public virtual Guid? CostId { get; set; }
        [ForeignKey("CostId")]
        public virtual Cost Cost { get; set; }//市價 
        public virtual Guid? ProcedureId { get; set; }
        [ForeignKey("ProcedureId")]
        public virtual MaterialProcedure Procedure { get; set; }
        
    }

    public class MaterialChange : Change<Material>
    {
    }

    //模擬工序(詢價產生 未投入實際生產 Process.ParentId=詢價ProcessId)
    //標準加工(工序需經主管審批 Process.ParentId is null)
    //買入實物(提領物料)
    public class MaterialProcedure : TypeSimpleDetail<Material>
    {
        public virtual Guid? EquipmentId { get; set; }
        [ForeignKey("EquipmentId")]
        public virtual Material Equipment { get; set; }//標準加工: Equipment.Cost * Time = 估算折舊成本; 買入實物: Equipment.Cost = 估算消耗物料成本
        public float? Time { get; set; }//耗時
        public virtual Guid? HumanId { get; set; }
        [ForeignKey("HumanId")]
        public virtual Cost Human { get; set; }//預估消耗人力

        public virtual Guid? PowerId { get; set; }
        [ForeignKey("PowerId")]
        public virtual Cost Power { get; set; }//預估消耗能源
    }

    //時間
    //金錢
    public class Cost : Period<Material>
    {
        public virtual Guid? UnitId { get; set; }
        [ForeignKey("UnitId")]
        public virtual Option Unit { get; set; }
        public float? Amount { get; set; }//能源成本
    }


    public class MaterialProcedureInput : Detail<MaterialProcedure>
    {
        public virtual Guid? InputId { get; set; }
        [ForeignKey("InputId")]
        public virtual Material Input { get; set; }
    }

    public class Recipe : SimpleDetail<Material>
    {
        public virtual Guid? ProcedureId { get; set; }
        [ForeignKey("ProcedureId")]
        public virtual Procedure Procedure { get; set; }//Material集合=BOM表

        
    }

    public class Procedure : SimpleDetail<Material>
    {
        
    }
}