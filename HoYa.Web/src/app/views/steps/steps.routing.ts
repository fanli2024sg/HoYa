import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { StepsComponent } from "./steps.component";
import { StepUpdateView } from "./update/stepUpdate.view";
export const stepsRoutes: Routes = [
    {
        path: "",
        component: StepsComponent,
        children: []
    },
    {
        path: ":id",
        component: StepUpdateView
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
