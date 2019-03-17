import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { EnquiriesEditComponent } from "./enquiries-edit.component";
export var enquiriesEditRoutes = [
    {
        path: "",
        component: EnquiriesEditComponent
    }
];
var EnquiriesEditRoutingModule = /** @class */ (function () {
    function EnquiriesEditRoutingModule() {
    }
    EnquiriesEditRoutingModule = tslib_1.__decorate([
        NgModule({
            imports: [
                RouterModule.forChild(enquiriesEditRoutes)
            ],
            exports: [
                RouterModule
            ]
        })
    ], EnquiriesEditRoutingModule);
    return EnquiriesEditRoutingModule;
}());
export { EnquiriesEditRoutingModule };
//# sourceMappingURL=enquiries-edit-routing.module.js.map