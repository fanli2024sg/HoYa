import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { CanDeactivateGuard } from "core/guards/can-deactivate.guard";
import { AuthGuard } from "core/guards/auth.guard";
const appRoutes: Routes = [
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

@NgModule({
    imports: [
        RouterModule.forRoot(
            appRoutes
        )
    ],
    exports: [
        RouterModule
    ],
    providers: [
        CanDeactivateGuard,
        AuthGuard
    ]
})
export class AppRoutingModule { }
