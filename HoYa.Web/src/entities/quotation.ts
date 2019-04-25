import { Detail, Base } from "./entity";
import { InquiryGeneral, Inquiry } from "./inquiry";

export class Quotation extends Detail<QuotationGeneral>
{
    amount: number;
    bargain: string;
    bargainPrice: number;
    inquiryId: string;
    inquiry: Inquiry;
    constructor() {
        super();
    }
}


export class QuotationGeneral extends Base {
    inquiryGeneralId: string;
    inquiryGeneral: InquiryGeneral;
    constructor() {
        super();
    }
}