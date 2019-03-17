import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { EnquiriesNewComponent } from "./enquiries-new.component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { EnquiriesNewRoutingModule } from "./enquiries-new-routing.module";
import { MatIconModule, MatButtonModule, MatCardModule, MatInputModule, MatFormFieldModule } from "@angular/material";
var EnquiriesNewModule = /** @class */ (function () {
    function EnquiriesNewModule() {
    }
    EnquiriesNewModule = tslib_1.__decorate([
        NgModule({
            imports: [
                FormsModule,
                ReactiveFormsModule,
                CommonModule,
                EnquiriesNewRoutingModule,
                MatIconModule,
                MatButtonModule,
                MatCardModule,
                MatInputModule,
                MatFormFieldModule
            ],
            declarations: [EnquiriesNewComponent]
        })
    ], EnquiriesNewModule);
    return EnquiriesNewModule;
}());
export { EnquiriesNewModule };
//# sourceMappingURL=enquiries-new.module.js.map