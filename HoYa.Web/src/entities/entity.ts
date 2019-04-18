import { Profile } from "./person";
export class Guid {
    static newGuid() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }
}

export abstract class Base {
    id: string;

    createdById: string;
    createdBy: Profile;

    createdDate: Date;

    constructor() {
        this.id = Guid.newGuid();
        this.createdDate = null;
    }
}

export class Change extends Base {
    constructor() {
        super();
    }
}

export abstract class Extention extends Base {
    updatedById: string;
    updatedBy: Profile;

    updatedDate: Date;

    constructor() {
        super();
        this.updatedDate = new Date();
    }
}

export abstract class Simple extends Extention {
    value: string;

    constructor() {
        super();
    }
}

export abstract class TypeExtention extends Extention {
    typeId: string;
    type: Option;

    constructor() {
        super();
    }
}


export abstract class Entity extends Extention {
    changeId: string;
    change: Change;

    constructor() {
        super();
    }
}

export abstract class Definition extends Entity {
    code: string;
    value: string;
    statusId: string;
    status: Option;

    constructor() {
        super();
    }
}

export abstract class NodeDefinition<P> extends Definition {
    parentId: string;
    parent: P;
    constructor() {
        super();
    }
}

export abstract class TypeDefinition extends Definition {
    typeId: string;
    type: Option;

    constructor() {
        super();
    }
}


export abstract class DefinitionDetail<D, DC> extends Definition {
    definitionId: string;
    definition: D;
    definitionChangeId: string;
    definitionChange: DC;
    constructor() {
        super();
    }
}

export abstract class General<D> extends Base {
    definitionId: string;
    definition: D;
    definitionChangeId: string;
    definitionChange: Change;
    constructor() {
        super();
    }
}

export abstract class SimpleGeneral<D> extends General<D>
{
    value: string;
    constructor() {
        super();
    }
}

export abstract class RealSimpleGeneral<D> extends SimpleGeneral<D>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}

export abstract class NodeGeneral<P, D> extends General<D>
{
    no: string;
    parentId: string;
    parent: P;
    constructor() {
        super();
    }
}


export abstract class Instance<DD> extends Base {
    no: string;
    definitionDetailId: string;
    definitionDetail: DD;
    entityChangeId: string;
    entityChange: Change;
    constructor() {
        super();
    }
}

export abstract class Detail<O> extends Base {
    ownerId: string;
    owner: O;
    ownerChangeId: string;
    ownerChange: Change;
    constructor() {
        super();
    }
}

export abstract class SimpleDetail<O> extends Detail<O> {
    value: string; 
    constructor() {
        super();
    }
}

export abstract class Relation<O, T> extends Detail<O>
{
    targetId: string;
    target: T;
    startDate: Date;
    endDate: Date;
    archivedDate: Date;
    archivedById: string;
    archivedBy: Profile;
    constructor() {
        super();
        this.archivedDate = new Date();
    }
}

export abstract class Event<O, OC, T, TC> extends Relation<O, T>
{
    targetChangeId: string;
    targetChange: TC;
    constructor() {
        super();
    }
}

export abstract class Node<P> extends Entity {
    parentId: string;
    parent: P;

    constructor() {
        super();
    }
}

export class Option extends NodeDefinition<Option>
{
    constructor() {
        super();
    }
}



export class Folder extends Base {
    value: string;

    constructor() {
        super();
    }
}
export class FolderFile extends Relation<Folder, File>
{
    constructor() {
        super();
    }
}
export class File extends Base {
    path: string;
    url: string;
    value: string;
    constructor() {
        super();
    }
}