import { NgModule } from "@angular/core";
import { InquiriesComponent } from "./inquiries.component";
import { InquiriesRouting } from "./inquiries.routing";
import { AppCommon } from "app/app.common";
@NgModule({
    imports: [
        InquiriesRouting,
        AppCommon
    ],
    declarations: [InquiriesComponent]
})

export class InquiriesModule { }
