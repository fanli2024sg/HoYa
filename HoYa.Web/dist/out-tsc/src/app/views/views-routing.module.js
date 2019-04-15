import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { ViewsComponent } from "./views.component";
import { AuthGuard } from "core/guards/auth.guard";
export var viewsRoutes = [
    {
        path: "",
        component: ViewsComponent,
        canActivate: [AuthGuard],
        children: [
            { path: "", redirectTo: "enquiries", pathMatch: "full" },
            {
                path: "enquiries",
                loadChildren: "./enquiries/enquiries.module#EnquiriesModule"
            }
        ]
    }
];
var ViewsRoutingModule = /** @class */ (function () {
    function ViewsRoutingModule() {
    }
    ViewsRoutingModule = tslib_1.__decorate([
        NgModule({
            imports: [
                RouterModule.forChild(viewsRoutes)
            ],
            exports: [
                RouterModule
            ]
        })
    ], ViewsRoutingModule);
    return ViewsRoutingModule;
}());
export { ViewsRoutingModule };
//# sourceMappingURL=views-routing.module.js.map