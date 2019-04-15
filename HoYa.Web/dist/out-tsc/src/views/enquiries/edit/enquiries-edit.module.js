import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
// App modules/components
import { NavigationModule } from "views/common/navigation/navigation.module";
import { FooterModule } from "views/common/footer/footer.module";
import { TopnavbarModule } from "views/common/topnavbar/topnavbar.module";
import { EnquiriesEditComponent } from "./enquiries-edit.component";
import { EnquiriesEditRoutingModule } from "./enquiries-edit-routing.module";
import { MatIconModule, MatButtonModule, MatButtonToggleModule, MatRippleModule, MatSidenavModule, MatToolbarModule, MatListModule } from "@angular/material";
var EnquiriesEditModule = /** @class */ (function () {
    function EnquiriesEditModule() {
    }
    EnquiriesEditModule = tslib_1.__decorate([
        NgModule({
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
    ], EnquiriesEditModule);
    return EnquiriesEditModule;
}());
export { EnquiriesEditModule };
//# sourceMappingURL=enquiries-edit.module.js.map