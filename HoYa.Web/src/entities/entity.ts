import { AspNetUser } from "./identity";
export abstract class Base<I>
{
    id: I;
    createdById: string;
    createdBy: AspNetUser;
    CreatedDate: Date;
    constructor() {
    }
}
export abstract class Association<O, T> extends Base<string>
{
    ownerId: string;
    owner: O;
    targetId: string;
    target: T;
    constructor() {
        super();
    }
}
export abstract class Event<O, T> extends Association<O, T>
{
    startDate: Date;
    endDate: Date;
    constructor() {
        super();
    }
}
export abstract class Entity extends Base<string>
{
    UpdatedById: string;
    UpdatedBy: AspNetUser;
    UpdatedDate: Date;
    constructor() {
        super();
    }
}
export abstract class TypeEntity extends Entity {
    typeId: string;
    type: Option;
    constructor() {
        super();
    }
}
export abstract class RealEntity extends Entity {
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class RealTypeEntity extends TypeEntity {
    GalleryId: string;
    Gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class Simple extends Entity {
    value: string;
    constructor() {
        super();
    }
}
export abstract class TypeSimple extends Simple {
    typeId: string;
    type: Option;
    constructor() {
        super();
    }
}
export abstract class RealSimple extends Simple {
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class RealTypeSimple extends TypeSimple {
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class Detail<O> extends Entity {
    ownerId: string;
    owner: O;
    constructor() {
        super();
    }
}
export abstract class TypeDetail<O> extends Detail<O>
{
    typeId: string;
    type: Option;
    constructor() {
        super();
    }
}
export abstract class RealDetail<O> extends Detail<O>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class RealTypeDetail<O> extends TypeDetail<O>
{
    galleryId: string;
    gallery: Folder;
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
export abstract class TypeSimpleDetail<O> extends SimpleDetail<O>
{
    typeId: string;
    type: Option;
    constructor() {
        super();
    }
}
export abstract class RealSimpleDetail<O> extends SimpleDetail<O>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class RealTypeSimpleDetail<O> extends TypeSimpleDetail<O>
{
    galleryId: string;
    gallery: Folder;
}
export abstract class Action<O> extends Detail<O> {
    ownerId: string;
    owner: O;
    no: string;
    actionDate: Date;
    constructor() {
        super();
    }
}
export abstract class TypeAction<O> extends Action<O>
{
    typeId: string;
    type: Option;
    constructor() {
        super();
    }
}
export abstract class RealAction<O> extends Action<O>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class RealTypeAction<O> extends TypeAction<O>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class SimpleAction<O> extends Action<O>
{
    value: string;
    constructor() {
        super();
    }
}
export abstract class TypeSimpleAction<O> extends SimpleAction<O>
{
    typeId: string;
    type: Option;
    constructor() {
        super();
    }
}
export abstract class RealSimpleAction<O> extends SimpleAction<O>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class RealTypeSimpleAction<O> extends TypeSimpleAction<O>
{
    galleryId: string;
    gallery: Folder;
}
export abstract class Approval extends Entity {
    json: string;
    no: string;
    achivingReference: string;
    achivingDate: string;
    approvalDate: string;
    step: string;
    stepReference: string;
}
export abstract class Period<O> extends Detail<O>
{
    startDate: Date;
    endDate: Date;
    constructor() {
        super();
    }
}
export abstract class TypePeriod<O> extends Period<O>
{
    typeId: string;
    type: Option;
    constructor() {
        super();
    }
}
export abstract class RealPeriod<O> extends Period<O>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class RealTypePeriod<O> extends TypePeriod<O>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class SimplePeriod<O> extends Period<O>
{
    value: string;
    constructor() {
        super();
    }
}
export abstract class TypeSimplePeriod<O> extends SimplePeriod<O>
{
    typeId: string;
    type: Option;
    constructor() {
        super();
    }
}
export abstract class RealSimplePeriod<O> extends SimplePeriod<O>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class RealTypeSimplePeriod<O> extends TypeSimplePeriod<O>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class Record<O, T> extends Period<O>
{
    targetId: string;
    target: T;
    constructor() {
        super();
    }
}
export abstract class TypeRecord<O, T> extends Record<O, T>
{
    typeId: string;
    type: Option;
    constructor() {
        super();
    }
}
export abstract class RealRecord<O, T> extends Record<O, T>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class RealTypeRecord<O, T> extends TypeRecord<O, T>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class SimpleRecord<O, T> extends Record<O, T>
{
    value: string;
    constructor() {
        super();
    }
}
export abstract class TypeSimpleRecord<O, T> extends SimpleRecord<O, T>
{
    typeId: string;
    type: Option;
    constructor() {
        super();
    }
}
export abstract class RealSimpleRecord<O, T> extends SimpleRecord<O, T>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class RealTypeSimpleRecord<O, T> extends TypeSimpleRecord<O, T>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class Position<P> extends Entity {
    splitId: string;
    split: P;
    takeDate: Date;
    closeDate: Date;
    mergeId: string;
    merge: P;
    constructor() {
        super();
    }
}
export abstract class TypePosition<P> extends Position<P>
{
    typeId: string;
    type: Option;
    constructor() {
        super();
    }
}
export abstract class RealPosition<P> extends Position<P>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class RealTypePosition<P> extends TypePosition<P>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class SimplePosition<P> extends Position<P>
{
    value: string;
    constructor() {
        super();
    }
}
export abstract class TypeSimplePosition<P> extends SimplePosition<P>
{
    typeId: string;
    type: Option;
    constructor() {
        super();
    }
}
export abstract class RealSimplePosition<P> extends SimplePosition<P>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class RealTypeSimplePosition<P> extends TypeSimplePosition<P>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class PositionNode<P> extends Position<P>
{
    parentId: string;
    parent: P;
    constructor() {
        super();
    }
}
export abstract class TypePositionNode<P> extends PositionNode<P>
{
    typeId: string;
    type: Option;
    constructor() {
        super();
    }
}
export abstract class RealPositionNode<P> extends PositionNode<P>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class RealTypePositionNode<P> extends TypePositionNode<P>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class SimplePositionNode<P> extends PositionNode<P>
{
    value: string;
    constructor() {
        super();
    }
}
export abstract class TypeSimplePositionNode<P> extends SimplePositionNode<P>
{
    typeId: string;
    type: Option;
    constructor() {
        super();
    }
}
export abstract class RealSimplePositionNode<P> extends SimplePositionNode<P>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class RealTypeSimplePositionNode<P> extends TypeSimplePositionNode<P>
{
    galleryId: string;
    gallery: Folder;
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
export abstract class TypeNode<P> extends Node<P>
{
    typeId: string;
    type: Option;
    constructor() {
        super();
    }
}
export abstract class RealNode<P> extends Node<P>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class RealTypeNode<P> extends TypeNode<P>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class SimpleNode<P> extends Node<P>
{
    value: string;
    constructor() {
        super();
    }
}
export abstract class TypeSimpleNode<P> extends SimpleNode<P>
{
    typeId: string;
    type: Option;
    constructor() {
        super();
    }
}
export abstract class RealSimpleNode<P> extends SimpleNode<P>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export abstract class RealTypeSimpleNode<P> extends TypeSimpleNode<P>
{
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}
export class Option extends SimpleNode<Option> {
    code: string;
    options: Option[];
    unitId: string;
    unit: Option;
    sort: number;
    selected: boolean;
    _remark: any;
    _open: boolean;
    constructor(id?: string, value?: string, selected?: boolean, parentId?: string, _remark?: any, unit?: Option, code?: string) {
        super();
        this.id = id;
        this.value = value;
        this.selected = selected;
        this.parentId = parentId;
        this._remark = _remark;
        this.unit = unit;
        this.code = code;
    }
}
export class Folder extends Simple {
    folderFiles: FolderFile[];
    constructor() {
        super();
    }
}
export class FolderFile extends Association<Folder, File>
{
    constructor(ownerId?: string, targetId?: string) {
        super();
        this.ownerId = ownerId;
        this.targetId = targetId;
    }
}
export class File extends Simple {
    _display: string;
    value: string;
    name: string;
    constructor() {
        super();
    }
}


