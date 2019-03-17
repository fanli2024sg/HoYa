import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { CommonModule } from '@angular/common';
import { RouterModule } from "@angular/router";
import { NavigationComponent } from "./navigation.component";
var NavigationModule = /** @class */ (function () {
    function NavigationModule() {
    }
    NavigationModule = tslib_1.__decorate([
        NgModule({
            declarations: [NavigationComponent],
            imports: [
                CommonModule,
                RouterModule
            ],
            exports: [NavigationComponent],
        })
    ], NavigationModule);
    return NavigationModule;
}());
export { NavigationModule };
//# sourceMappingURL=navigation.module.js.map