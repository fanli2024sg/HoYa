import * as tslib_1 from "tslib";
import { TypeSimpleDetail, TypeNode, Entity } from "./entity";
var Process = /** @class */ (function (_super) {
    tslib_1.__extends(Process, _super);
    function Process(type) {
        var _this = _super.call(this) || this;
        _this.type = type;
        _this.no = "　";
        return _this;
    }
    return Process;
}(TypeNode));
export { Process };
var General = /** @class */ (function (_super) {
    tslib_1.__extends(General, _super);
    function General() {
        return _super.call(this) || this;
    }
    return General;
}(Entity));
export { General };
var Change = /** @class */ (function (_super) {
    tslib_1.__extends(Change, _super);
    function Change() {
        return _super.call(this) || this;
    }
    return Change;
}(Entity));
export { Change };
var Mission = /** @class */ (function (_super) {
    tslib_1.__extends(Mission, _super); //TypeId=一般作業 特殊作業 緊急臨時特殊作業 Type.ParentId= 訪客 上課 施工
    function Mission() {
        return _super.call(this) || this;
    }
    return Mission;
}(TypeSimpleDetail));
export { Mission };
//# sourceMappingURL=process.js.map