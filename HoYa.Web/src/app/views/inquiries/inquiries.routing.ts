import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { InquiriesComponent } from "./inquiries.component";

export const inquiriesRoutes: Routes = [
    {
        path: "",
        component: InquiriesComponent,
        children: [
            { path: "", redirectTo: "generals", pathMatch: "full" },
            {
                path: "generals",
                loadChildren: "./generals/inquiryGenerals.module#InquiryGeneralsModule"
            }
        ]
    }
];
@NgModule({
    imports: [
        RouterModule.forChild(inquiriesRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class InquiriesRouting { }
