import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ViewsComponent } from "./views.component";
import { AuthGuard } from "guards/auth.guard";

export const viewsRoutes: Routes = [
    {
        path: "",
        component: ViewsComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "",
                redirectTo: "inquiries",
                pathMatch: "full"
            },
            {
                path: "inquiries",
                loadChildren: "./inquiries/inquiries.module#InquiriesModule"
            },
            {
                path: "profiles",
                loadChildren: "./profiles/profiles.module#ProfilesModule"
            },
            {
                path: "steps",
                loadChildren: "./steps/steps.module#StepsModule"
            }
        ]
    }
];
@NgModule({
    imports: [
        RouterModule.forChild(viewsRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class ViewsRouting { }
