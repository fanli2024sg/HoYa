import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { LoginComponent } from "./login.component";
var loginRoutes = [
    {
        path: "",
        component: LoginComponent
    }
];
var LoginRoutingModule = /** @class */ (function () {
    function LoginRoutingModule() {
    }
    LoginRoutingModule = tslib_1.__decorate([
        NgModule({
            imports: [
                RouterModule.forChild(loginRoutes)
            ],
            exports: [
                RouterModule
            ]
        })
    ], LoginRoutingModule);
    return LoginRoutingModule;
}());
export { LoginRoutingModule };
//# sourceMappingURL=login-routing.module.js.map