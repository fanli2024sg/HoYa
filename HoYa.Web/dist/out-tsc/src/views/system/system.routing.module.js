import * as tslib_1 from "tslib";
//核心
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
//物件
import { SystemComponent } from "./system.component";
//插件
var systemRoutes = [
    {
        path: "", component: SystemComponent,
        children: [
        /* { path: "", redirectTo: "main", pathMatch: "full" },
         {
             path: "processes",
             loadChildren: "views/system/processes/processes.module#ProcessesModule"
         },
         {
             path: "main",
             loadChildren: "views/system/main/main.module#MainModule"
         },
         {
             path: "options",
             loadChildren: "views/system/options/options.module#OptionsModule"
         }*/
        ]
    }
];
var SystemRoutingModule = /** @class */ (function () {
    function SystemRoutingModule() {
    }
    SystemRoutingModule = tslib_1.__decorate([
        NgModule({
            imports: [
                RouterModule.forChild(systemRoutes)
            ],
            exports: [
                RouterModule
            ]
        })
    ], SystemRoutingModule);
    return SystemRoutingModule;
}());
export { SystemRoutingModule };
//# sourceMappingURL=system.routing.module.js.map