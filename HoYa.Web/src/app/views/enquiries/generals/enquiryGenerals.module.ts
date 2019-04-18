import { NgModule } from "@angular/core";
import { EnquiryGeneralsComponent } from "./enquiryGenerals.component";
import { EnquiryGeneralUpdateComponent } from "./update/enquiryGeneralUpdate.component";
import { EnquiryGeneralCreateComponent } from "./create/enquiryGeneralCreate.component";
import { EnquiryAppendComponent } from "../append/enquiryAppend.component";
import { ProfileAppendComponent } from "../../profiles/append/profileAppend.component";
import { EnquiryGeneralsRouting } from "./enquiryGenerals.routing";
import { AppCommon } from "app/app.common";
import { ReactiveFormsModule } from "@angular/forms";
@NgModule({
    imports: [
        ReactiveFormsModule,
        AppCommon,
        EnquiryGeneralsRouting
    ],
    declarations: [
        EnquiryAppendComponent, 
        ProfileAppendComponent,
        EnquiryGeneralsComponent,
        EnquiryGeneralUpdateComponent,
        EnquiryGeneralCreateComponent
    ],
    entryComponents: [
        EnquiryAppendComponent,
        ProfileAppendComponent
    ]
})

export class EnquiryGeneralsModule { }