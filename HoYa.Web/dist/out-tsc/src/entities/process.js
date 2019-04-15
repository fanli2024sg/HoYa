import * as tslib_1 from "tslib";
import { NodeGeneral, Definition, Detail, Instance } from "./entity";
var WorkFlow = /** @class */ (function (_super) {
    tslib_1.__extends(WorkFlow, _super);
    function WorkFlow() {
        return _super.call(this) || this;
    }
    return WorkFlow;
}(Definition));
export { WorkFlow };
var Step = /** @class */ (function (_super) {
    tslib_1.__extends(Step, _super);
    function Step() {
        return _super.call(this) || this;
    }
    return Step;
}(Detail));
export { Step };
var Mission = /** @class */ (function (_super) {
    tslib_1.__extends(Mission, _super);
    function Mission() {
        return _super.call(this) || this;
    }
    return Mission;
}(Instance));
export { Mission };
var Process = /** @class */ (function (_super) {
    tslib_1.__extends(Process, _super);
    function Process(definitionId, definitionChangeId) {
        var _this = _super.call(this) || this;
        _this.definitionId = definitionId;
        _this.definitionChangeId = definitionChangeId;
        return _this;
    }
    return Process;
}(NodeGeneral));
export { Process };
//# sourceMappingURL=process.js.map