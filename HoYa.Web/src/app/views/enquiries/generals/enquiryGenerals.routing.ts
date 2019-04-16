import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { EnquiryGeneralsComponent } from "./enquiryGenerals.component";
import { EnquiryGeneralCreateComponent } from "./create/enquiryGeneralCreate.component";
import { EnquiryGeneralUpdateComponent } from "./update/enquiryGeneralUpdate.component";

export const enquiryGeneralsRoutes: Routes = [
    {
        path: "",
        component: EnquiryGeneralsComponent
    },
    {
        path: "create",
        component: EnquiryGeneralCreateComponent
    },
    {
        path: ":id",
        component: EnquiryGeneralUpdateComponent
    }
];
@NgModule({
    imports: [
        RouterModule.forChild(enquiryGeneralsRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class EnquiryGeneralsRouting { }
