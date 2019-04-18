import { Extention, Detail } from "./entity";
import { Process } from "./workflow";
import { Material } from "./material";
import { Profile } from "./person";
export class Enquiry extends Detail<EnquiryGeneral>
{
    materialId: string;
    material: Material;
    minAmount: number;
    quantity: number;
    price: number;
    priceMin: number;
    priceMax: number;
    constructor(ownerId?:string) {
        super();
        this.ownerId = ownerId;
        this.quantity = 1;
    }
}

export class EnquiryGeneral extends Extention {
    profileId: string;
    profile: Profile;
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