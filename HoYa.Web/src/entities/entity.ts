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

    _checked: boolean;
    constructor() {
        this.id = Guid.newGuid();
        this.createdDate = null;
    }
}
export abstract class Definition extends Base {
    code: string;
    value: string;
    statusId: string;
    status: Option;
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
export abstract class RealTypeDefinition extends TypeDefinition {
    galleryId: string;
    gallery: Folder;
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
export abstract class RealNodeDefinition<P> extends NodeDefinition<P>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class TypeNodeDefinition<P> extends NodeDefinition<P>
{
    typeId: string;
    type: Option;
    constructor() {
        super();
    }
}
export class Branch<D> extends Base {
    code: string;
    value: string;
    statusId: string;
    status: Option;
    definitionId: string;
    definition: D;
    constructor() {
        super();
    }
}
export class Change<DB> extends Base {
    definitionBranchId: string;
    definitionBranch: DB;
    constructor() {
        super();
    }
}
export class Detail<O> extends Base {
    ownerId: string;
    owner: O;
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
    }
}
export abstract class SimpleDetail<O> extends Detail<O>
{
    value: string;
    constructor() {
        super();
    }
}
export class TypeSimpleDetail<O> extends SimpleDetail<O> {
    typeId: string;
    type: Option;
    constructor() {
        super();
    }
}
export class Instance<DB, DC> extends Base {
    no: string;
    definitionBranchId: string;
    definitionBranch: DB;
    definitionChangeId: string;
    definitionChange: DC;
    constructor() {
        super();
    }
}
export abstract class SimpleInstance<DB, DC> extends Instance<DB, DC>
{
    value: string;
    constructor() {
        super();
    }
}
export abstract class TypeSimpleInstance<DB, DC> extends SimpleInstance<DB, DC>
{
    typeId: string;
    type: Option;
    constructor() {
        super();
    }
}
export abstract class RealSimpleInstance<DB, DC> extends SimpleInstance<DB, DC>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class RealTypeSimpleInstance<DB, DC> extends TypeSimpleInstance<DB, DC>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class NodeInstance<B, C, P> extends Instance<B, C>
{
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
export class Category extends RealNodeDefinition<Category>
{
    constructor() {
        super();
    }
}
export class Folder extends Definition {
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
export class File extends Definition {
    path: string;
    url: string;
    constructor() {
        super();
    }
}
