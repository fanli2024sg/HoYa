import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { CanDeactivateGuard } from "@core/guards/can-deactivate.guard";
import { AuthGuard } from "@core/guards/auth.guard";
var appRoutes = [
    { path: "", redirectTo: "contents", pathMatch: "full" },
    {
        path: "login",
        loadChildren: "app/login/login.module#LoginModule"
    },
    {
        path: "contents",
        loadChildren: "app/contents/contents.module#ContentsModule",
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
                CanDeactivateGuard
            ]
        })
    ], AppRoutingModule);
    return AppRoutingModule;
}());
export { AppRoutingModule };
//# sourceMappingURL=app-routing.module.js.map