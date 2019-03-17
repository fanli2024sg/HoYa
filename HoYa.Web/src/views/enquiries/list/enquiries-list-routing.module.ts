import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { EnquiriesListComponent } from "./enquiries-list.component";
export const enquiriesListRoutes: Routes = [
    {
        path: "",
        component: EnquiriesListComponent
    }
];
@NgModule({
    imports: [
        RouterModule.forChild(enquiriesListRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class EnquiriesListRoutingModule { }
