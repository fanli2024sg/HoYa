using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{
    public class Inquiry : Detail<InquiryGeneral>//Process.Type.Value=="Inquiry"
    { 
        public virtual Guid? RecipeId { get; set; }
        [ForeignKey("RecipeId")]
        public virtual Recipe Recipe { get; set; }//原料耗材能源設備折舊成本
        public float? Price { get; set; }

        public string Delivery { get; set; }
        public string Remark { get; set; }
        public float? Quantity { get; set; }
    }

    public class InquiryGeneral : SimpleDetail<Process>
    {
        public virtual Guid? ContactId { get; set; }
        [ForeignKey("ContactId")]
        public virtual Contact Contact { get; set; }

        public virtual Guid? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Person Customer { get; set; }
    }
}