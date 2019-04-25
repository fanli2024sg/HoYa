import { TypeNodeDefinition, Relation, Base, Detail, NodeDefinition } from "./entity";

export class Group extends TypeNodeDefinition<Group> {
    constructor() {
        super();
    }
}

//可用功能
export class FunctionGroup extends Relation<Function, Group>
{
    constructor() {
        super();
    }
}

/// <summary>
/// 可用功能
/// </summary>
export class Function extends Base {
}

//群組規則
export class Rule extends Detail<Group>
{
    constructor() {
        super();
    }
}

//選單
export class Menu extends NodeDefinition<Menu>
{
    value: string;
    code: string;
    functionId: string;
    function: Function;
    constructor() {
        super();
    }
}