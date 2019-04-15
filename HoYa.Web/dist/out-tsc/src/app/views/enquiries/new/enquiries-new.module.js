import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { EnquiriesNewComponent } from "./enquiries-new.component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { EnquiriesNewRoutingModule } from "./enquiries-new-routing.module";
import { CoreModule } from "core/core.module";
var EnquiriesNewModule = /** @class */ (function () {
    function EnquiriesNewModule() {
    }
    EnquiriesNewModule = tslib_1.__decorate([
        NgModule({
            imports: [
                FormsModule,
                ReactiveFormsModule,
                EnquiriesNewRoutingModule,
                CoreModule
            ],
            declarations: [EnquiriesNewComponent]
        })
    ], EnquiriesNewModule);
    return EnquiriesNewModule;
}());
export { EnquiriesNewModule };
//# sourceMappingURL=enquiries-new.module.js.map