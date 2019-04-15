import * as tslib_1 from "tslib";
import { TypeSimple, Simple, Association, SimpleDetail, SimpleNode } from "./entity";
import { Change } from "./process";
//公共授權 type=ehs
//門禁授權 type=gate
//部門建立 type=hr
var Group = /** @class */ (function (_super) {
    tslib_1.__extends(Group, _super);
    function Group() {
        return _super.call(this) || this;
    }
    return Group;
}(TypeSimple));
export { Group };
//公共授權 type=ehs
//門禁授權 type=gate
//部門建立 type=hr
var GroupChange = /** @class */ (function (_super) {
    tslib_1.__extends(GroupChange, _super);
    function GroupChange() {
        return _super.call(this) || this;
    }
    return GroupChange;
}(Change));
export { GroupChange };
//可用功能
var FunctionGroup = /** @class */ (function (_super) {
    tslib_1.__extends(FunctionGroup, _super);
    function FunctionGroup() {
        return _super.call(this) || this;
    }
    return FunctionGroup;
}(Association));
export { FunctionGroup };
/// <summary>
/// 可用功能
/// </summary>
var Function = /** @class */ (function (_super) {
    tslib_1.__extends(Function, _super);
    function Function() {
        return _super.call(this) || this;
    }
    return Function;
}(Simple));
export { Function };
//群組規則
var Rule = /** @class */ (function (_super) {
    tslib_1.__extends(Rule, _super);
    function Rule() {
        return _super.call(this) || this;
    }
    return Rule;
}(SimpleDetail));
export { Rule };
//選單
var Menu = /** @class */ (function (_super) {
    tslib_1.__extends(Menu, _super);
    function Menu() {
        return _super.call(this) || this;
    }
    return Menu;
}(SimpleNode));
export { Menu };
//# sourceMappingURL=group.js.map