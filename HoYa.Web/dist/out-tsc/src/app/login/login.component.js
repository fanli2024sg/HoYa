import * as tslib_1 from "tslib";
import { Component } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { AuthService } from "core/services/auth.service";
var LoginComponent = /** @class */ (function () {
    function LoginComponent(authService, router, formBuilder) {
        this.authService = authService;
        this.router = router;
        this.formBuilder = formBuilder;
        this.aspNetUser = formBuilder.group({
            // 定義表格的預設值
            "value": [null, Validators.compose([Validators.required, Validators.minLength(6)])],
            "password": [null, Validators.compose([Validators.required, Validators.minLength(6)])]
        });
    }
    LoginComponent.prototype.login = function (aspNetUser) {
        var _this = this;
        this.authService.login(aspNetUser.value, aspNetUser.password).subscribe(function () {
            if (_this.authService.isLoggedIn()) {
                var redirect = _this.authService.redirectUrl ? _this.authService.redirectUrl : "/views";
                _this.router.navigate([redirect]);
            }
        });
    };
    LoginComponent.prototype.adlogin = function () {
        var _this = this;
        this.authService.ad().subscribe(function () {
            if (_this.authService.isLoggedIn()) {
                var redirect = _this.authService.redirectUrl ? _this.authService.redirectUrl : "/views";
                _this.router.navigate([redirect]);
            }
        });
    };
    LoginComponent.prototype.logout = function () {
        this.authService.logout();
    };
    LoginComponent = tslib_1.__decorate([
        Component({
            selector: "login",
            templateUrl: "login.component.html",
            styleUrls: ['login.component.css']
        }),
        tslib_1.__metadata("design:paramtypes", [AuthService,
            Router,
            FormBuilder])
    ], LoginComponent);
    return LoginComponent;
}());
export { LoginComponent };
//# sourceMappingURL=login.component.js.map