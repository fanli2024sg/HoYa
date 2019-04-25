using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{

    public class Quotation : Detail<QuotationGeneral>//Process.Type.Value=="O"
    {
        public float? Amount { get; set; }
        public string Bargain { get; set; }
        public float? BargainPrice { get; set; }
        public virtual Guid? InquiryId { get; set; }
        [ForeignKey("InquiryId")]
        public virtual Inquiry Inquiry { get; set; }
    }


    public class QuotationGeneral : Base//Process.Type.Value=="O"
    {
        public virtual Guid? InquiryGeneralId { get; set; }
        [ForeignKey("InquiryGeneralId")]
        public virtual InquiryGeneral InquiryGeneral { get; set; }
    }
}