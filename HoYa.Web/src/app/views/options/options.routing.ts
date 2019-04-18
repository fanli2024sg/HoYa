import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { OptionsComponent } from "./options.component";
import { OptionCreateComponent } from "./create/optionCreate.component";
import { OptionUpdateComponent } from "./update/optionUpdate.component";

export const optionsRoutes: Routes = [
    {
        path: "",
        component: OptionsComponent
    },
    {
        path: "create",
        component: OptionCreateComponent
    },
    {
        path: ":id",
        component: OptionUpdateComponent
    }
];
@NgModule({
    imports: [
        RouterModule.forChild(optionsRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class OptionsRouting { }
