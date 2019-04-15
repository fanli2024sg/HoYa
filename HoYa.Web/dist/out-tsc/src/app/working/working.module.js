import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { CommonModule } from '@angular/common';
import { WorkingComponent } from './working.component';
import { FormsModule } from '@angular/forms';
import { WorkingRoutingModule } from './working.routing.module';
var WorkingModule = /** @class */ (function () {
    function WorkingModule() {
    }
    WorkingModule = tslib_1.__decorate([
        NgModule({
            imports: [
                FormsModule,
                CommonModule,
                WorkingRoutingModule
            ],
            declarations: [WorkingComponent]
        })
    ], WorkingModule);
    return WorkingModule;
}());
export { WorkingModule };
//# sourceMappingURL=working.module.js.map