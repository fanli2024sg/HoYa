using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{

    public class Quotation : Detail<QuotationGeneral>//Process.Type.Value=="O"
    {
        public float? Amount { get; set; }
        public string Bargain { get; set; }
        public float? BargainPrice { get; set; }
        public virtual Guid? EnquiryId { get; set; }
        [ForeignKey("EnquiryId")]
        public virtual Enquiry Enquiry { get; set; }
    }


    public class QuotationGeneral : Extention//Process.Type.Value=="O"
    {
        public virtual Guid? EnquiryGeneralId { get; set; }
        [ForeignKey("EnquiryGeneralId")]
        public virtual EnquiryGeneral EnquiryGeneral { get; set; }
    }
}