import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { EnquiriesComponent } from "./enquiries.component";

export const enquiriesRoutes: Routes = [
    {
        path: "",
        component: EnquiriesComponent,
        children: [
            { path: "", redirectTo: "list", pathMatch: "full" },
            {
                path: "list",
                loadChildren: "./list/enquiries-list.module#EnquiriesListModule"
            },
            {
                path: "new",
                loadChildren: "./new/enquiries-new.module#EnquiriesNewModule"
            },
            {
                path: ":id",
                loadChildren: "./edit/enquiries-edit.module#EnquiriesEditModule"
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
