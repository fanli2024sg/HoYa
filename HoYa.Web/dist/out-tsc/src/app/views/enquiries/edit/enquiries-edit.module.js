import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { EnquiriesEditComponent } from "./enquiries-edit.component";
import { EnquiriesEditRoutingModule } from "./enquiries-edit-routing.module";
import { CoreModule } from "core/core.module";
var EnquiriesEditModule = /** @class */ (function () {
    function EnquiriesEditModule() {
    }
    EnquiriesEditModule = tslib_1.__decorate([
        NgModule({
            imports: [
                EnquiriesEditRoutingModule,
                CoreModule
            ],
            declarations: [EnquiriesEditComponent]
        })
    ], EnquiriesEditModule);
    return EnquiriesEditModule;
}());
export { EnquiriesEditModule };
//# sourceMappingURL=enquiries-edit.module.js.map