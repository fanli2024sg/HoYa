using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{
    //實例化Material
    public class Inventory : Instance<Material>
    {
      
    }
    public class InventoryChange : Change<Inventory>
    {
        public virtual Guid? MaterialProcedureId { get; set; }
        [ForeignKey("MaterialProcedureId")]
        public virtual Recipe MaterialProcedure { get; set; }

        public virtual Guid? MergeId { get; set; }
        [ForeignKey("MergeId")]
        public virtual Inventory Merge { get; set; }
        public string Remark { get; set; }
        public float Amount { get; set; }
    }
}