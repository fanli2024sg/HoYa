import { Extention, Detail } from "./entity";
import { Process } from "./process";
import { Material } from "./material";
export class Enquiry extends Detail<EnquiryGeneral>
{
    materialId: string;
    material: Material;
    minAmount: number;
    quantity: number;
    constructor(ownerId?:string) {
        super();
        this.ownerId = ownerId;
        this.quantity = 1;
    }
}

export class EnquiryGeneral extends Extention {
    processId: string;
    process: Process;
    contactPerson: string;
    contactValue: string;
    customerName: string;
    content: string;
    constructor() {
        super();
    }
}