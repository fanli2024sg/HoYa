import * as tslib_1 from "tslib";
import { Component } from "@angular/core";
import { smoothlyMenu } from "@core/helpers/app.helpers";
import { Router } from "@angular/router";
import { AuthService } from "@core/services/auth.service";
var TopnavbarComponent = /** @class */ (function () {
    function TopnavbarComponent(router, authService) {
        this.router = router;
        this.authService = authService;
    }
    TopnavbarComponent.prototype.toggleNavigation = function () {
        $("body").toggleClass("mini-navbar");
        smoothlyMenu();
    };
    TopnavbarComponent.prototype.logout = function () {
        this.authService.logout();
        this.router.navigate(['/login']);
    };
    TopnavbarComponent = tslib_1.__decorate([
        Component({
            selector: 'topnavbar',
            templateUrl: 'topnavbar.template.html'
        }),
        tslib_1.__metadata("design:paramtypes", [Router,
            AuthService])
    ], TopnavbarComponent);
    return TopnavbarComponent;
}());
export { TopnavbarComponent };
//# sourceMappingURL=topnavbar.component.js.map