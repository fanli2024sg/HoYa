import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { WorkingComponent } from './working.component';
var workingRoutes = [
    { path: "", component: WorkingComponent }
];
var WorkingRoutingModule = /** @class */ (function () {
    function WorkingRoutingModule() {
    }
    WorkingRoutingModule = tslib_1.__decorate([
        NgModule({
            imports: [
                RouterModule.forChild(workingRoutes)
            ],
            exports: [
                RouterModule
            ]
        })
    ], WorkingRoutingModule);
    return WorkingRoutingModule;
}());
export { WorkingRoutingModule };
//# sourceMappingURL=working.routing.module.js.map