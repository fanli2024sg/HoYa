import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { EnquiriesComponent } from "./enquiries.component";
export var enquiriesRoutes = [
    {
        path: "",
        component: EnquiriesComponent,
        children: [
            { path: "", redirectTo: "new", pathMatch: "full" },
            {
                path: "list",
                loadChildren: "views/enquiries/list/enquiries-list.module#EnquiriesListModule"
            },
            {
                path: "new",
                loadChildren: "views/enquiries/new/enquiries-new.module#EnquiriesNewModule"
            },
            {
                path: ":id",
                loadChildren: "views/enquiries/edit/enquiries-edit.module#EnquiriesEditModule"
            }
        ]
    }
];
var EnquiriesRoutingModule = /** @class */ (function () {
    function EnquiriesRoutingModule() {
    }
    EnquiriesRoutingModule = tslib_1.__decorate([
        NgModule({
            imports: [
                RouterModule.forChild(enquiriesRoutes)
            ],
            exports: [
                RouterModule
            ]
        })
    ], EnquiriesRoutingModule);
    return EnquiriesRoutingModule;
}());
export { EnquiriesRoutingModule };
//# sourceMappingURL=enquiries-routing.module.js.map