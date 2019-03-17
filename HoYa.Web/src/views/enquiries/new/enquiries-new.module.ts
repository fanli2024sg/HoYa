import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { EnquiriesNewComponent } from "./enquiries-new.component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { EnquiriesNewRoutingModule } from "./enquiries-new-routing.module";
import {
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatInputModule,
    MatFormFieldModule
} from "@angular/material";

import { MatFileUploadModule } from "angular-material-fileupload";
@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        EnquiriesNewRoutingModule,
        MatIconModule,
        MatButtonModule,
        MatCardModule,
        MatInputModule,
        MatFormFieldModule,
        MatFileUploadModule
    ],
    declarations: [EnquiriesNewComponent]
})
export class EnquiriesNewModule { }
