import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { LoginComponent } from "./login.component";
import { ReactiveFormsModule } from "@angular/forms";
import { LoginRoutingModule } from "./login-routing.module";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
var LoginModule = /** @class */ (function () {
    function LoginModule() {
    }
    LoginModule = tslib_1.__decorate([
        NgModule({
            imports: [
                FormsModule,
                CommonModule,
                ReactiveFormsModule,
                LoginRoutingModule,
                CommonModule
            ],
            declarations: [LoginComponent]
        })
    ], LoginModule);
    return LoginModule;
}());
export { LoginModule };
//# sourceMappingURL=login.module.js.map