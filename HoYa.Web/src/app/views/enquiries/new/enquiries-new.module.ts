import { NgModule } from "@angular/core";
import { EnquiriesNewComponent } from "./enquiries-new.component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { EnquiriesNewRoutingModule } from "./enquiries-new-routing.module";
import { CoreModule } from "core/core.module";
@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        EnquiriesNewRoutingModule,
        CoreModule
    ],
    declarations: [EnquiriesNewComponent]
})
export class EnquiriesNewModule { }
