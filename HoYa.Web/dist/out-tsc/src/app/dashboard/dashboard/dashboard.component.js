import * as tslib_1 from "tslib";
import { Component } from '@angular/core';
var DashboardComponent = /** @class */ (function () {
    function DashboardComponent() {
    }
    DashboardComponent.prototype.ngOnInit = function () { };
    DashboardComponent.prototype.toggleSideNav = function (sideNav) {
        sideNav.toggle().then(function (result) {
            console.log(result);
            console.log("\u9078\u55AE\u72C0\u614B\uFF1A" + result.type);
        });
    };
    DashboardComponent.prototype.opened = function () {
        console.log('芝麻開門');
    };
    DashboardComponent.prototype.closed = function () {
        console.log('芝麻關門');
    };
    DashboardComponent = tslib_1.__decorate([
        Component({
            selector: 'app-dashboard',
            templateUrl: './dashboard.component.html',
            styleUrls: ['./dashboard.component.css']
        }),
        tslib_1.__metadata("design:paramtypes", [])
    ], DashboardComponent);
    return DashboardComponent;
}());
export { DashboardComponent };
//# sourceMappingURL=dashboard.component.js.map