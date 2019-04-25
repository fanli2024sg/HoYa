import { NgModule } from "@angular/core";
import { InquiryGeneralsComponent } from "./inquiryGenerals.component";
import { InquiryGeneralUpdateView } from "./update/inquiryGeneralUpdate.view";
import { InquiryGeneralCreateDialog } from "./create/inquiryGeneralCreate.dialog";
import { InquiryAppendComponent } from "../append/inquiryAppend.component";
import { ProfileAppendDialog } from "../../profiles/append/profileAppend.dialog";
import { RecipeAppendDialog } from "../../recipes/append/recipeAppend.dialog";
import { InquiryGeneralsRouting } from "./inquiryGenerals.routing";
import { AppCommon } from "app/app.common";
import { ReactiveFormsModule } from "@angular/forms";
@NgModule({
    imports: [
        ReactiveFormsModule,
        AppCommon,
        InquiryGeneralsRouting
    ],
    declarations: [
        InquiryAppendComponent, 
        ProfileAppendDialog,
        RecipeAppendDialog,
        InquiryGeneralsComponent,
        InquiryGeneralUpdateView,
        InquiryGeneralCreateDialog
    ],
    entryComponents: [
        InquiryAppendComponent,
        ProfileAppendDialog,
        RecipeAppendDialog,
        InquiryGeneralCreateDialog
    ]
})

export class InquiryGeneralsModule { }
