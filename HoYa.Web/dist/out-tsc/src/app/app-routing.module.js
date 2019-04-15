import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { CanDeactivateGuard } from "core/guards/can-deactivate.guard";
import { AuthGuard } from "core/guards/auth.guard";
var appRoutes = [
    { path: "", redirectTo: "views", pathMatch: "full" },
    {
        path: "login",
        loadChildren: "./login/login.module#LoginModule"
    },
    {
        path: "views",
        loadChildren: "./views/views.module#ViewsModule",
        canLoad: [AuthGuard]
    }
];
var AppRoutingModule = /** @class */ (function () {
    function AppRoutingModule() {
    }
    AppRoutingModule = tslib_1.__decorate([
        NgModule({
            imports: [
                RouterModule.forRoot(appRoutes)
            ],
            exports: [
                RouterModule
            ],
            providers: [
                CanDeactivateGuard,
                AuthGuard
            ]
        })
    ], AppRoutingModule);
    return AppRoutingModule;
}());
export { AppRoutingModule };
//# sourceMappingURL=app-routing.module.js.map