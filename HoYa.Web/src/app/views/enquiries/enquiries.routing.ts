import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { EnquiriesComponent } from "./enquiries.component";

export const enquiriesRoutes: Routes = [
    {
        path: "",
        component: EnquiriesComponent,
        children: [
            { path: "", redirectTo: "generals", pathMatch: "full" },
            {
                path: "generals",
                loadChildren: "./generals/enquiryGenerals.module#EnquiryGeneralsModule"
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
export class EnquiriesRouting { }
