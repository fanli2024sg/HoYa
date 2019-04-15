//核心
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
//服務
import { AuthGuard } from "@core/guards/auth.guard";
//物件
import { SystemComponent } from "./system.component";
//插件
const systemRoutes: Routes = [
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

@NgModule({
    imports: [
        RouterModule.forChild(systemRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class SystemRoutingModule { }
