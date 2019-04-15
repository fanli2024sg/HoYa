import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { EnquiriesComponent } from "./enquiries.component";

export const enquiriesRoutes: Routes = [
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
@NgModule({
    imports: [
        RouterModule.forChild(enquiriesRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class EnquiriesRoutingModule { }
