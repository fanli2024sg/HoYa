import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { ViewsComponent } from "./views.component";
import { ViewsRoutingModule } from "./views-routing.module";
import { AuthGuard } from "core/guards/auth.guard";
var ViewsModule = /** @class */ (function () {
    function ViewsModule() {
    }
    ViewsModule = tslib_1.__decorate([
        NgModule({
            imports: [
                CommonModule,
                FormsModule,
                ViewsRoutingModule,
                CommonModule
            ],
            declarations: [ViewsComponent], providers: [AuthGuard]
        })
    ], ViewsModule);
    return ViewsModule;
}());
export { ViewsModule };
//# sourceMappingURL=views.module.js.map