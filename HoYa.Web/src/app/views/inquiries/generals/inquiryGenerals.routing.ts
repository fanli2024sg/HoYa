import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { InquiryGeneralsComponent } from "./inquiryGenerals.component";
import { InquiryGeneralUpdateView } from "./update/inquiryGeneralUpdate.view";

export const inquiryGeneralsRoutes: Routes = [
    {
        path: "",
        component: InquiryGeneralsComponent
    },
    {
        path: ":id",
        component: InquiryGeneralUpdateView
    }
];
@NgModule({
    imports: [
        RouterModule.forChild(inquiryGeneralsRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class InquiryGeneralsRouting { }
