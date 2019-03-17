import { Option, TypeSimpleDetail, TypeNode, Entity } from "./entity";
import { Profile } from "./person";
export class Process extends TypeNode<Process>
{
    no: string;
    mission: string;
    overTime: Date;
    constructor(type?: Option) {
        super();
        this.type = type;
        this.no = "　";
    }
}


export abstract class General extends Entity {
    processId: string;
    process: Process;
    constructor() {
        super();
    }
}

export abstract class Change<E> extends Entity {
    processId: string;
    process: Process;

    entityId: string;
    entity: E;
    constructor() {
        super();
    }
}


export class Mission extends TypeSimpleDetail<Profile>//TypeId=一般作業 特殊作業 緊急臨時特殊作業 Type.ParentId= 訪客 上課 施工
{
    processId: string;
    process: Process;
    comment: string;
    actionId: string;
    action: Option;
    actionDate: Date;
    overTime: Date;
    constructor() {
        super();
    }
}

