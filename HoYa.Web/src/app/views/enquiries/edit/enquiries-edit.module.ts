import { NgModule } from "@angular/core";
import { EnquiriesEditComponent } from "./enquiries-edit.component";
import { ReactiveFormsModule } from "@angular/forms";
import { EnquiriesEditRoutingModule } from "./enquiries-edit-routing.module";
import { EnquiryComponent } from "./add/enquiry.component";
import { CoreModule } from "core/core.module";

@NgModule({
    imports: [
        ReactiveFormsModule,
        EnquiriesEditRoutingModule,
        CoreModule
    ],
    declarations: [EnquiriesEditComponent, EnquiryComponent],
    entryComponents: [EnquiryComponent]
})


export class EnquiriesEditModule { }

