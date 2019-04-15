import * as tslib_1 from "tslib";
import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { AuthService } from "core/services/auth.service";
var ViewsComponent = /** @class */ (function () {
    function ViewsComponent(router, authService) {
        this.router = router;
        this.authService = authService;
        this.title = "";
    }
    ViewsComponent.prototype.ngOnInit = function () { };
    ViewsComponent.prototype.toggleSideNav = function (sideNav) {
        sideNav.toggle().then(function (result) {
            console.log(result);
            console.log("\uFFFD\uFFFD\u6AAC\uFFFDA\uFFFDG" + result.type);
        });
    };
    ViewsComponent.prototype.opened = function () {
        console.log("open");
    };
    ViewsComponent.prototype.closed = function () {
        console.log("close");
    };
    ViewsComponent.prototype.goto = function (title) {
        this.title = title;
    };
    ViewsComponent.prototype.back = function () {
        this.title = "";
    };
    ViewsComponent.prototype.logout = function () {
        this.authService.logout();
        this.router.navigate(["./login"]);
    };
    ViewsComponent = tslib_1.__decorate([
        Component({
            selector: "views",
            templateUrl: "views.component.html",
            styleUrls: ["views.component.css"],
            providers: []
        }),
        tslib_1.__metadata("design:paramtypes", [Router,
            AuthService])
    ], ViewsComponent);
    return ViewsComponent;
}());
export { ViewsComponent };
//# sourceMappingURL=views.component.js.map