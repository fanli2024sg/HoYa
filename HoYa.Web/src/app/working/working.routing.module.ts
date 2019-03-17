import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { WorkingComponent } from './working.component';

const workingRoutes: Routes = [
    { path: "", component: WorkingComponent }
];

@NgModule({
    imports: [
        RouterModule.forChild(workingRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class WorkingRoutingModule { }
