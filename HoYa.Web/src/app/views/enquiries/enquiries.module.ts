import { NgModule } from "@angular/core";
import { EnquiriesComponent } from "./enquiries.component";
import { EnquiriesRouting } from "./enquiries.routing";
import { AppCommon } from "app/app.common";
@NgModule({
    imports: [
        EnquiriesRouting,
        AppCommon
    ],
    declarations: [EnquiriesComponent]
})

export class EnquiriesModule { }