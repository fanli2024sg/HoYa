import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { EnquiriesNewComponent } from './enquiries-new.component';

const enquiriesNewRoutes: Routes = [
    { path: '', component: EnquiriesNewComponent }
];

@NgModule({
    imports: [
        RouterModule.forChild(enquiriesNewRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class EnquiriesNewRoutingModule { }