import * as tslib_1 from "tslib";
import { TypeSimpleDetail, Detail, TypeSimple, Period, SimpleDetail } from "./entity";
import { Change } from "./process";
//設備
//物料
var Material = /** @class */ (function (_super) {
    tslib_1.__extends(Material, _super);
    function Material() {
        return _super.call(this) || this;
    }
    return Material;
}(TypeSimple));
export { Material };
var MaterialChange = /** @class */ (function (_super) {
    tslib_1.__extends(MaterialChange, _super);
    function MaterialChange() {
        return _super.call(this) || this;
    }
    return MaterialChange;
}(Change));
export { MaterialChange };
//模擬工序(詢價產生 未投入實際生產 Process.ParentId=詢價ProcessId)
//標準加工(工序需經主管審批 Process.ParentId is null)
//買入實物(提領物料)
var MaterialProcedure = /** @class */ (function (_super) {
    tslib_1.__extends(MaterialProcedure, _super);
    function MaterialProcedure() {
        var _this = _super.call(this) || this;
        _this.equipment = new Material();
        return _this;
    }
    return MaterialProcedure;
}(TypeSimpleDetail));
export { MaterialProcedure };
//時間
//金錢
var Cost = /** @class */ (function (_super) {
    tslib_1.__extends(Cost, _super);
    function Cost() {
        return _super.call(this) || this;
    }
    return Cost;
}(Period));
export { Cost };
var MaterialProcedureInput = /** @class */ (function (_super) {
    tslib_1.__extends(MaterialProcedureInput, _super);
    function MaterialProcedureInput() {
        return _super.call(this) || this;
    }
    return MaterialProcedureInput;
}(Detail));
export { MaterialProcedureInput };
var Recipe = /** @class */ (function (_super) {
    tslib_1.__extends(Recipe, _super);
    function Recipe() {
        return _super.call(this) || this;
    }
    return Recipe;
}(SimpleDetail));
export { Recipe };
var Procedure = /** @class */ (function (_super) {
    tslib_1.__extends(Procedure, _super);
    function Procedure() {
        return _super.call(this) || this;
    }
    return Procedure;
}(SimpleDetail));
export { Procedure };
//# sourceMappingURL=material.js.map