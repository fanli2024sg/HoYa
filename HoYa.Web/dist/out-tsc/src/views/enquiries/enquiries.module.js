import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
// App modules/components
import { NavigationModule } from "views/common/navigation/navigation.module";
import { FooterModule } from "views/common/footer/footer.module";
import { TopnavbarModule } from "views/common/topnavbar/topnavbar.module";
import { EnquiriesComponent } from "./enquiries.component";
import { EnquiriesRoutingModule } from "./enquiries-routing.module";
import { MatIconModule, MatButtonModule, MatButtonToggleModule, MatRippleModule, MatSidenavModule, MatToolbarModule, MatListModule } from "@angular/material";
var EnquiriesModule = /** @class */ (function () {
    function EnquiriesModule() {
    }
    EnquiriesModule = tslib_1.__decorate([
        NgModule({
            imports: [
                CommonModule,
                FormsModule,
                NavigationModule,
                FooterModule,
                TopnavbarModule,
                EnquiriesRoutingModule,
                MatButtonModule,
                MatIconModule,
                MatButtonModule,
                MatButtonToggleModule,
                MatRippleModule,
                MatSidenavModule,
                MatToolbarModule,
                MatListModule
            ],
            declarations: [EnquiriesComponent]
        })
    ], EnquiriesModule);
    return EnquiriesModule;
}());
export { EnquiriesModule };
//# sourceMappingURL=enquiries.module.js.map