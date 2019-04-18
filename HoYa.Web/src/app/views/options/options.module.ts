import { NgModule } from "@angular/core";
import { OptionsComponent } from "./options.component";
import { OptionUpdateComponent } from "./update/optionUpdate.component";
import { OptionCreateComponent } from "./create/optionCreate.component";
import { OptionsRouting } from "./options.routing";
import { AppCommon } from "app/app.common";
import { ReactiveFormsModule } from "@angular/forms";
@NgModule({
    imports: [
        ReactiveFormsModule,
        AppCommon,
        OptionsRouting
    ],
    declarations: [
        OptionsComponent,
        OptionUpdateComponent,
        OptionCreateComponent
    ],
    entryComponents: [
        OptionCreateComponent
    ]
})

export class OptionsModule { }