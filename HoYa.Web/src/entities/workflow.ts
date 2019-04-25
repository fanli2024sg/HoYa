import { Branch, Change, Definition, SimpleDetail, NodeInstance, Relation } from "./entity";
import { Group } from "./group";
import { Profile } from "./person";


export class WorkFlowDefinition extends Definition {
    constructor() {
        super();
    }
}


export class WorkFlow extends Branch<WorkFlowDefinition>{
    constructor() {
        super();
    }
}
export class WorkFlowChange extends Change<WorkFlow>
{
    constructor() {
        super();
    }
}

export class Step extends SimpleDetail<WorkFlow>
{
    constructor() {
        super();
    }
}
export class StepGroup extends Relation<Step, Group>
{
    constructor() {
        super();
    }
}



export class Activity extends Relation<Process, Step>
{
    participantId: string;
    participant: Profile;
    constructor() {
        super();
    }
}

export class Process extends NodeInstance<WorkFlow, WorkFlowChange, Process>
{
    constructor(definitionBranchId?: string, definitionChangeId?: string) {
        super();
        this.definitionBranchId = definitionBranchId;
        this.definitionChangeId = definitionChangeId;
    }
}