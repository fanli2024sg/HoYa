import * as tslib_1 from "tslib";
import { Component, ViewChild } from "@angular/core";
import { MatIconRegistry, MatRipple } from "@angular/material";
import { DomSanitizer } from "@angular/platform-browser";
var EnquiriesComponent = /** @class */ (function () {
    function EnquiriesComponent(matIconRegistry, domSanitizer) {
        this.matIconRegistry = matIconRegistry;
        this.domSanitizer = domSanitizer;
    }
    EnquiriesComponent.prototype.ngOnInit = function () {
        this.matIconRegistry.addSvgIconInNamespace("custom-svg", "angular", this.domSanitizer.bypassSecurityTrustResourceUrl("assets/images/angular_solidBlack.svg"));
        this.matIconRegistry.registerFontClassAlias("fontawesome", "fa");
    };
    EnquiriesComponent.prototype.triggerRipple = function () {
        var point1 = this.ripple.launch(0, 0, { color: "pink", centered: true, persistent: true, radius: 50 });
        var point2 = this.ripple.launch(0, 0, { color: "yellow", centered: true, persistent: true, radius: 20 });
        setTimeout(function () {
            point1.fadeOut();
        }, 500);
    };
    EnquiriesComponent.prototype.clearRipple = function () {
        this.ripple.fadeOutAll();
    };
    tslib_1.__decorate([
        ViewChild(MatRipple),
        tslib_1.__metadata("design:type", MatRipple)
    ], EnquiriesComponent.prototype, "ripple", void 0);
    EnquiriesComponent = tslib_1.__decorate([
        Component({
            selector: "enquiries",
            templateUrl: "./enquiries.component.html"
        }),
        tslib_1.__metadata("design:paramtypes", [MatIconRegistry, DomSanitizer])
    ], EnquiriesComponent);
    return EnquiriesComponent;
}());
export { EnquiriesComponent };
//# sourceMappingURL=enquiries.component.js.map