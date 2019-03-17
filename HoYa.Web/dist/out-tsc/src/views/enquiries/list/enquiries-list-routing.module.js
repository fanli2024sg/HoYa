import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { EnquiriesListComponent } from "./enquiries-list.component";
export var enquiriesListRoutes = [
    {
        path: "",
        component: EnquiriesListComponent
    }
];
var EnquiriesListRoutingModule = /** @class */ (function () {
    function EnquiriesListRoutingModule() {
    }
    EnquiriesListRoutingModule = tslib_1.__decorate([
        NgModule({
            imports: [
                RouterModule.forChild(enquiriesListRoutes)
            ],
            exports: [
                RouterModule
            ]
        })
    ], EnquiriesListRoutingModule);
    return EnquiriesListRoutingModule;
}());
export { EnquiriesListRoutingModule };
//# sourceMappingURL=enquiries-list-routing.module.js.map