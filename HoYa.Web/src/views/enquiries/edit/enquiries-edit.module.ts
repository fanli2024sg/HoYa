import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";

// App modules/components
import { NavigationModule } from "views/common/navigation/navigation.module";
import { FooterModule } from "views/common/footer/footer.module";
import { TopnavbarModule } from "views/common/topnavbar/topnavbar.module";
import { EnquiriesEditComponent } from "./enquiries-edit.component"; 
import { EnquiriesEditRoutingModule } from "./enquiries-edit-routing.module";
import {
    MatIconModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatRippleModule,
    MatSidenavModule,
    MatToolbarModule,
    MatListModule
} from "@angular/material"; 
@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        NavigationModule,
        FooterModule,
        TopnavbarModule,
        EnquiriesEditRoutingModule,
        MatButtonModule,
        MatIconModule,
        MatButtonModule,
        MatButtonToggleModule,
        MatRippleModule,
        MatSidenavModule,
        MatToolbarModule,
        MatListModule
    ],
    declarations: [EnquiriesEditComponent]
})

export class EnquiriesEditModule { }