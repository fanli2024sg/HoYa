import { Detail } from "./entity";
import { EnquiryGeneral, Enquiry } from "./enquiry";
import { Process, General } from "entities/process";

export class Quotation extends Detail<QuotationGeneral>//Process.Type.Value=="O"
{
    amount: number;
    bargain: string;
    bargainPrice : number;
    enquiryId: string;
    enquiry: Enquiry;
    constructor() {
        super();
    }
}

export class QuotationGeneral extends General//Process.Type.Value=="O"
{
    enquiryGeneralId: string;
    enquiryGeneral: EnquiryGeneral;
    _hasOrderPackage: boolean;
    constructor(process?: Process) {
        super();
        this.process = process;
    }
}
