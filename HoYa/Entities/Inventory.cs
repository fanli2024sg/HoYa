using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{
    //實例化Material
    public class Inventory : Period<Inventory>
    {
        public virtual Guid? StorageId { get; set; }
        [ForeignKey("StorageId")]
        public virtual Storage Storage { get; set; }

        public virtual Guid? MaterialProcedureId { get; set; }
        [ForeignKey("MaterialProcedureId")]
        public virtual MaterialProcedure MaterialProcedure { get; set; }

        public virtual Guid? MergeId { get; set; }
        [ForeignKey("MergeId")]
        public virtual Inventory Merge  { get; set; }
        public string Remark { get; set; }
        public float Amount { get; set; }
    }
    public class InventoryChange : Change<Inventory>
    {
    }

    public class Storage : TypeNode<Storage>
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class StorageChange : Change<Storage>
    {
    }
}