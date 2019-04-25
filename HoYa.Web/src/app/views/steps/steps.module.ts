import { NgModule } from "@angular/core";
import { StepsComponent } from "./steps.component";
import { StepCreateDialog } from "./create/stepCreate.dialog";
import { StepUpdateView } from "./update/stepUpdate.view";
import { StepsRouting } from "./steps.routing";
import { AppCommon } from "app/app.common";
@NgModule({
    imports: [       
        StepsRouting,
        AppCommon
    ],
    declarations: [
        StepCreateDialog,
        StepsComponent,
        StepUpdateView
    ],
    entryComponents: [
        StepCreateDialog 
    ]
})

export class StepsModule { }