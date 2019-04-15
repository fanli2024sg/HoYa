import { Entity, SimpleNode, Association } from "./entity";

/// <summary>
/// 帳號群組
/// </summary>
export class Group extends Entity {
    value: string;
    constructor() {
        super();
    }
}
/// <summary>
/// 帳號群組 
/// 有多個 可用功能
/// </summary>
export class GroupFunction extends Association<Group, Function>
{
    constructor(owner?: Group) {
        super();
        this.owner = owner;
    }
}
/// <summary>
/// 可用功能
/// </summary>
export class Function extends Entity {
    value: string;
    groupFunctions: GroupFunction[];
    constructor() {
        super();
    }
}
/// <summary>
/// 群組規則 
/// 有一個 帳號群組
/// </summary>
export class Rule extends Entity {
    groupId: string;
    group: Group;
    constructor() {
        super();
    }
}
/// <summary>
/// 選單 
/// 有一個 可用功能
/// 有一個 上層選單
/// </summary>
export class Menu extends SimpleNode<Menu>
{
    _active: boolean;
    functionId: string;
    function: Function;
    menus: Menu[];
    constructor() {
        super();
    }
}
