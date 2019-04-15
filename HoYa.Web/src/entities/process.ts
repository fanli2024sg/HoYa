import { NodeGeneral, Definition, Detail, Instance } from "./entity";
import { Group } from "./group";
import { Profile } from "./person";
export class WorkFlow extends Definition {
    constructor() {
        super();
    }
}


export class Step extends Detail<WorkFlow>
{
    constructor() {
        super();
    }
}

export class Mission extends Instance<Step>
{
    processId: string;
    process: Process;

    groupId: string;
    group: Group;

    ProfileId: string;
    profile: Profile;
    constructor() {
        super();
    }
}

export class Process extends NodeGeneral<Process, WorkFlow>
{
    constructor(definitionId?: string, definitionChangeId?: string) {
        super();
        this.definitionId = definitionId;
        this.definitionChangeId = definitionChangeId;
    }
}