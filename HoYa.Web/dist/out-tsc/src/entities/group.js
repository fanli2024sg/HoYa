import * as tslib_1 from "tslib";
import { TypeDefinition, Relation, Base, Detail, Node } from "./entity";
var Group = /** @class */ (function (_super) {
    tslib_1.__extends(Group, _super);
    function Group() {
        return _super.call(this) || this;
    }
    return Group;
}(TypeDefinition));
export { Group };
//可用功能
var FunctionGroup = /** @class */ (function (_super) {
    tslib_1.__extends(FunctionGroup, _super);
    function FunctionGroup() {
        return _super.call(this) || this;
    }
    return FunctionGroup;
}(Relation));
export { FunctionGroup };
/// <summary>
/// 可用功能
/// </summary>
var Function = /** @class */ (function (_super) {
    tslib_1.__extends(Function, _super);
    function Function() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return Function;
}(Base));
export { Function };
//群組規則
var Rule = /** @class */ (function (_super) {
    tslib_1.__extends(Rule, _super);
    function Rule() {
        return _super.call(this) || this;
    }
    return Rule;
}(Detail));
export { Rule };
//選單
var Menu = /** @class */ (function (_super) {
    tslib_1.__extends(Menu, _super);
    function Menu() {
        return _super.call(this) || this;
    }
    return Menu;
}(Node));
export { Menu };
//# sourceMappingURL=group.js.map