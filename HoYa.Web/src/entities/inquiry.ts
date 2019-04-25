import { SimpleDetail,Detail } from "./entity";
import { Process } from "./workflow";
import { Recipe } from "./item";
import { Person, Contact } from "./person";
export class Inquiry extends Detail<InquiryGeneral>
{
    recipeId: string;
    recipe: Recipe;
    minAmount: number;
    quantity: number;
    price: number;

    delivery: string;
    remark: string;
    constructor(ownerId?:string) {
        super();
        this.ownerId = ownerId;
        this.quantity = 1;
    }
}

export class InquiryGeneral extends SimpleDetail<Process> {
    contactId: string;
    contact: Contact;
    customerId: string;
    customer: Person;
    constructor(ownerDefinitionBranchId?: string, ownerDefinitionChangeId?: string) {
        super();
        this.owner = new Process(ownerDefinitionBranchId, ownerDefinitionChangeId);
        this.contact = new Contact();
        this.contact.owner = new Person();
        this.customer = new Person();
    }
}