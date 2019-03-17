using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{
    public class Enquiry : Detail<EnquiryGeneral>//Process.Type.Value=="Enquiry"
    {
        public string OtherMaterialProcedures { get; set; }
        public virtual Guid? MaterialProcedureId { get; set; }
        [ForeignKey("MaterialProcedureId")]
        public virtual Recipe MaterialProcedure { get; set; }//原料耗材能源設備折舊成本
        //新MaterialProcedure去產生MaterialChange Process => Process.Type.Value=="MaterialChange" ParentId=本Enquiry Process
        public float? MinAmount { get; set; }//主管壓底價
        public float? Quantity { get; set; }
    }

    public class EnquiryGeneral : General//Process.Type.Value=="O"
    {

        public string ContactPerson { get; set; }
        public string ContactValue { get; set; }
        public string CustomerName { get; set; }
        public string Content { get; set; }

    }
}