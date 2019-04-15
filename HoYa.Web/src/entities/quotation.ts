import { Detail, Extention } from "./entity";
import { EnquiryGeneral, Enquiry } from "./enquiry";

export class Quotation extends Detail<QuotationGeneral>
{
    amount: number;
    bargain: string;
    bargainPrice: number;
    enquiryId: string;
    enquiry: Enquiry;
    constructor() {
        super();
    }
}


export class QuotationGeneral extends Extention {
    enquiryGeneralId: string;
    enquiryGeneral: EnquiryGeneral;
    constructor() {
        super();
    }
}