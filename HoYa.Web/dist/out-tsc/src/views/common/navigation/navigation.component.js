import * as tslib_1 from "tslib";
import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { AuthService } from "@core/services/auth.service";
import { SettingsService } from "services/settings.service";
var NavigationComponent = /** @class */ (function () {
    function NavigationComponent(router, authService, settingsService) {
        var _this = this;
        this.router = router;
        this.authService = authService;
        this.settingsService = settingsService;
        this.activeLink = 1;
        this.currentUser = JSON.parse(localStorage.getItem("user"));
        if (this.currentUser.userId) {
            this.settingsService.find().subscribe(function (x) {
                _this.selectedProfile = x.profile;
            });
        }
    }
    NavigationComponent.prototype.ngOnInit = function () {
    };
    NavigationComponent.prototype.ngAfterViewInit = function () {
        //$("#side-menu").metisMenu();
    };
    NavigationComponent.prototype.activedRoute = function (route) {
        return this.router.url.indexOf(route) > -1;
    };
    NavigationComponent.prototype.getAuth = function () {
    };
    NavigationComponent.prototype.logout = function () {
        this.authService.logout();
        localStorage.removeItem("currentUser");
        this.router.navigate(["/login"]);
    };
    NavigationComponent = tslib_1.__decorate([
        Component({
            selector: 'navigation',
            templateUrl: 'navigation.template.html',
            providers: [AuthService, SettingsService]
        }),
        tslib_1.__metadata("design:paramtypes", [Router,
            AuthService,
            SettingsService])
    ], NavigationComponent);
    return NavigationComponent;
}());
export { NavigationComponent };
//# sourceMappingURL=navigation.component.js.map