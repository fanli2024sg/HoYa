import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { CanDeactivateGuard } from "@core/guards/can-deactivate.guard";
import { AuthGuard } from "@core/guards/auth.guard";
const appRoutes: Routes = [
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
        CanDeactivateGuard
    ]
})
export class AppRoutingModule { }
