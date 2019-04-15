import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { EnquiriesComponent } from "./enquiries.component";
import { EnquiriesRoutingModule } from "./enquiries-routing.module";
import { CoreModule } from "core/core.module";
var EnquiriesModule = /** @class */ (function () {
    function EnquiriesModule() {
    }
    EnquiriesModule = tslib_1.__decorate([
        NgModule({
            imports: [
                EnquiriesRoutingModule, CoreModule
            ],
            declarations: [EnquiriesComponent]
        })
    ], EnquiriesModule);
    return EnquiriesModule;
}());
export { EnquiriesModule };
//# sourceMappingURL=enquiries.module.js.map