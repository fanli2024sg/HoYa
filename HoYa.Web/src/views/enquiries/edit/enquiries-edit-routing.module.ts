import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { EnquiriesEditComponent } from "./enquiries-edit.component"; 
export const enquiriesEditRoutes: Routes = [
    {
        path: "",
        component: EnquiriesEditComponent
    }
];
@NgModule({
    imports: [
        RouterModule.forChild(enquiriesEditRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class EnquiriesEditRoutingModule { }
