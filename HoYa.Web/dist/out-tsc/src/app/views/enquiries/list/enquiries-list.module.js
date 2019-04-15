import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { EnquiriesListComponent } from "./enquiries-list.component";
import { EnquiriesListRoutingModule } from "./enquiries-list-routing.module";
import { CoreModule } from "core/core.module";
var EnquiriesListModule = /** @class */ (function () {
    function EnquiriesListModule() {
    }
    EnquiriesListModule = tslib_1.__decorate([
        NgModule({
            imports: [
                CoreModule,
                EnquiriesListRoutingModule
            ],
            declarations: [EnquiriesListComponent]
        })
    ], EnquiriesListModule);
    return EnquiriesListModule;
}());
export { EnquiriesListModule };
//# sourceMappingURL=enquiries-list.module.js.map