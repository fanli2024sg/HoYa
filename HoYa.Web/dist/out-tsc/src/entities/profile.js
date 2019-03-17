import * as tslib_1 from "tslib";
import { Entity, SimpleNode, Association } from "./entity";
/// <summary>
/// 帳號群組
/// </summary>
var Group = /** @class */ (function (_super) {
    tslib_1.__extends(Group, _super);
    function Group() {
        return _super.call(this) || this;
    }
    return Group;
}(Entity));
export { Group };
/// <summary>
/// 帳號群組 
/// 有多個 可用功能
/// </summary>
var GroupFunction = /** @class */ (function (_super) {
    tslib_1.__extends(GroupFunction, _super);
    function GroupFunction(owner) {
        var _this = _super.call(this) || this;
        _this.owner = owner;
        return _this;
    }
    return GroupFunction;
}(Association));
export { GroupFunction };
/// <summary>
/// 可用功能
/// </summary>
var Function = /** @class */ (function (_super) {
    tslib_1.__extends(Function, _super);
    function Function() {
        return _super.call(this) || this;
    }
    return Function;
}(Entity));
export { Function };
/// <summary>
/// 群組規則 
/// 有一個 帳號群組
/// </summary>
var Rule = /** @class */ (function (_super) {
    tslib_1.__extends(Rule, _super);
    function Rule() {
        return _super.call(this) || this;
    }
    return Rule;
}(Entity));
export { Rule };
/// <summary>
/// 選單 
/// 有一個 可用功能
/// 有一個 上層選單
/// </summary>
var Menu = /** @class */ (function (_super) {
    tslib_1.__extends(Menu, _super);
    function Menu() {
        return _super.call(this) || this;
    }
    return Menu;
}(SimpleNode));
export { Menu };
//# sourceMappingURL=profile.js.map