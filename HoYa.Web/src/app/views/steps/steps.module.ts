import { NgModule } from "@angular/core";
import { StepsComponent } from "./steps.component";
import { StepCreateDialog } from "./create/stepCreate.dialog";
import { StepsRouting } from "./steps.routing";
import { AppCommon } from "app/app.common";
@NgModule({
    imports: [       
        StepsRouting,
        AppCommon
    ],
    declarations: [
        StepCreateDialog,
        StepsComponent
    ],
    entryComponents: [
        StepCreateDialog 
    ]
})

export class StepsModule { }