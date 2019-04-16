using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{
    public class Enquiry : Detail<EnquiryGeneral>//Process.Type.Value=="Enquiry"
    { 
        public virtual Guid? MaterialId { get; set; }
        [ForeignKey("MaterialId")]
        public virtual Material Material { get; set; }//原料耗材能源設備折舊成本
        public float? MinAmount { get; set; }//主管壓底價
        public float? Quantity { get; set; }
    }

    public class EnquiryGeneral : Extention
    {
        public virtual Guid? ProfileId { get; set; }
        [ForeignKey("ProfileId")]
        public virtual Profile Profile { get; set; }
        public virtual Guid? ProcessId { get; set; }
        [ForeignKey("ProcessId")]
        public virtual Process Process { get; set; }
        public string ContactPerson { get; set; }
        public string ContactValue { get; set; }
        public string CustomerName { get; set; }
        public string Content { get; set; }
    }
}