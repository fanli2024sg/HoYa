import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { LoginComponent } from "./login.component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { LoginRoutingModule } from "./login-routing.module";
import { MatIconModule, MatButtonModule, MatCardModule, MatInputModule, MatFormFieldModule } from "@angular/material";
var LoginModule = /** @class */ (function () {
    function LoginModule() {
    }
    LoginModule = tslib_1.__decorate([
        NgModule({
            imports: [
                FormsModule,
                ReactiveFormsModule,
                CommonModule,
                LoginRoutingModule,
                MatIconModule,
                MatButtonModule,
                MatCardModule,
                MatInputModule,
                MatFormFieldModule
            ],
            declarations: [LoginComponent]
        })
    ], LoginModule);
    return LoginModule;
}());
export { LoginModule };
//# sourceMappingURL=login.module.js.map