import { Entity, Option, Detail } from "./entity";
import { Group } from "./group";
import { Person } from "./person";
import { Process } from "./process";
import { MaterialProcedure } from "./material";
export class Enquiry extends Detail<EnquiryGeneral>//Process.Type.Value=="Enquiry"
{
    processId: string;
    process: Process;
    materialProcedureId: string;
    materialProcedure: MaterialProcedure;//原料耗材能源設備折舊成本
    //新MaterialProcedure去產生MaterialChange Process => Process.Type.Value=="MaterialChange" ParentId=本Enquiry Process
    minAmount: number;//主管壓底價
    quantity: number;
    _index: string;
    constructor() {
        super();
        this.materialProcedure = new MaterialProcedure();
    }
}

export class EnquiryGeneral extends Entity//Process.Type.Value=="O"
{
    processId: string;
    process: Process;
    customerName: string;
    contactValue: string;
    contactPerson: string;
    content: string;
    constructor(process?: Process) {
        super();
        this.process = process;
    }
}
