import { NgModule } from "@angular/core";
import { EnquiriesListComponent } from "./enquiries-list.component";
import { EnquiriesListRoutingModule } from "./enquiries-list-routing.module";
import { CoreModule } from "core/core.module";
@NgModule({
    imports: [
        CoreModule,
        EnquiriesListRoutingModule
    ],
    declarations: [EnquiriesListComponent]
})

export class EnquiriesListModule { }