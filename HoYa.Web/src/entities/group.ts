import { TypeSimple, Entity, Option, Simple, Association, SimpleDetail, SimpleNode } from "./entity";
import { Change, Process } from "./process";
//公共授權 type=ehs
//門禁授權 type=gate
//部門建立 type=hr
export class Group extends TypeSimple {
    code: string;
    statusId: string;
    status: Option;
    changeId: string;
    change: GroupChange;
    startDate: Date;
    endDate: Date;
    constructor() {
        super();
    }
}

//公共授權 type=ehs
//門禁授權 type=gate
//部門建立 type=hr
export class GroupChange extends Change<Group>
{
    typeId: string;
    gateIds: string;
    startDate: Date;
    endDate: Date;
    constructor() {
        super();
    }
}

//可用功能
export class FunctionGroup extends Association<Function, Group>
{
    constructor() {
        super();
    }
}
/// <summary>
/// 可用功能
/// </summary>
export class Function extends Simple {
    constructor() {
        super();
    }
}
//群組規則
export class Rule extends SimpleDetail<Group>
{
    constructor() {
        super();
    }
}
//選單
export class Menu extends SimpleNode<Menu>
{
    functionId: string;
    function: Function;
    constructor() {
        super();
    }
}
