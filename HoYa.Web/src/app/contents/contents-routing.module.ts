import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ContentsComponent } from "./contents.component";
import { AuthGuard } from "@core/guards/auth.guard";

export const contentsRoutes: Routes = [
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
@NgModule({
    imports: [
        RouterModule.forChild(contentsRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class ContentsRoutingModule { }
