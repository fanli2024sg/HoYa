import * as tslib_1 from "tslib";
import { Detail } from "./entity";
import { General } from "entities/process";
var Quotation = /** @class */ (function (_super) {
    tslib_1.__extends(Quotation, _super); //Process.Type.Value=="O"
    function Quotation() {
        return _super.call(this) || this;
    }
    return Quotation;
}(Detail));
export { Quotation };
var QuotationGeneral = /** @class */ (function (_super) {
    tslib_1.__extends(QuotationGeneral, _super); //Process.Type.Value=="O"
    function QuotationGeneral(process) {
        var _this = _super.call(this) || this;
        _this.process = process;
        return _this;
    }
    return QuotationGeneral;
}(General //Process.Type.Value=="O"
));
export { QuotationGeneral };
//# sourceMappingURL=quotation.js.map