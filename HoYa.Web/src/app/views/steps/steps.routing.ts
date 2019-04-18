import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { StepsComponent } from "./steps.component";

export const stepsRoutes: Routes = [
    {
        path: "",
        component: StepsComponent,
        children: []
    }
];
@NgModule({
    imports: [
        RouterModule.forChild(stepsRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class StepsRouting { }
