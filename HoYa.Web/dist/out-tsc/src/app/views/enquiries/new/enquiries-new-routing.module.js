import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { EnquiriesNewComponent } from './enquiries-new.component';
var enquiriesNewRoutes = [
    { path: '', component: EnquiriesNewComponent }
];
var EnquiriesNewRoutingModule = /** @class */ (function () {
    function EnquiriesNewRoutingModule() {
    }
    EnquiriesNewRoutingModule = tslib_1.__decorate([
        NgModule({
            imports: [
                RouterModule.forChild(enquiriesNewRoutes)
            ],
            exports: [
                RouterModule
            ]
        })
    ], EnquiriesNewRoutingModule);
    return EnquiriesNewRoutingModule;
}());
export { EnquiriesNewRoutingModule };
//# sourceMappingURL=enquiries-new-routing.module.js.map