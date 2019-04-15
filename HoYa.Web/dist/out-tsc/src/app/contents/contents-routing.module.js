import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { ContentsComponent } from "./contents.component";
import { AuthGuard } from "@core/guards/auth.guard";
export var contentsRoutes = [
    {
        path: "", component: ContentsComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "enquiries",
                loadChildren: "views/enquiries/enquiries.module#EnquiriesModule"
            }
        ]
    }
];
var ContentsRoutingModule = /** @class */ (function () {
    function ContentsRoutingModule() {
    }
    ContentsRoutingModule = tslib_1.__decorate([
        NgModule({
            imports: [
                RouterModule.forChild(contentsRoutes)
            ],
            exports: [
                RouterModule
            ]
        })
    ], ContentsRoutingModule);
    return ContentsRoutingModule;
}());
export { ContentsRoutingModule };
//# sourceMappingURL=contents-routing.module.js.map