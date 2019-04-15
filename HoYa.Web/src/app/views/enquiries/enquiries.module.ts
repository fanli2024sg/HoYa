import { NgModule } from "@angular/core";
import { EnquiriesComponent } from "./enquiries.component";
import { EnquiriesRoutingModule } from "./enquiries-routing.module";
import { CoreModule } from "core/core.module";
@NgModule({
    imports: [
        EnquiriesRoutingModule,CoreModule
    ],
    declarations: [EnquiriesComponent]
})

export class EnquiriesModule { }