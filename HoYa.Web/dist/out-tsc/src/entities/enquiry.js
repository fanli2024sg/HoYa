import * as tslib_1 from "tslib";
import { Entity, Detail } from "./entity";
import { MaterialProcedure } from "./material";
var Enquiry = /** @class */ (function (_super) {
    tslib_1.__extends(Enquiry, _super); //Process.Type.Value=="Enquiry"
    function Enquiry() {
        var _this = _super.call(this) || this;
        _this.materialProcedure = new MaterialProcedure();
        return _this;
    }
    return Enquiry;
}(Detail));
export { Enquiry };
var EnquiryGeneral = /** @class */ (function (_super) {
    tslib_1.__extends(EnquiryGeneral, _super); //Process.Type.Value=="O"
    function EnquiryGeneral(process) {
        var _this = _super.call(this) || this;
        _this.process = process;
        return _this;
    }
    return EnquiryGeneral;
}(Entity //Process.Type.Value=="O"
));
export { EnquiryGeneral };
//# sourceMappingURL=enquiry.js.map