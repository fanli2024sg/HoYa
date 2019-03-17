import * as tslib_1 from "tslib";
import { Component, ViewChild } from "@angular/core";
import { MatIconRegistry, MatRipple } from "@angular/material";
import { DomSanitizer } from "@angular/platform-browser";
var EnquiriesListComponent = /** @class */ (function () {
    function EnquiriesListComponent(matIconRegistry, domSanitizer) {
        this.matIconRegistry = matIconRegistry;
        this.domSanitizer = domSanitizer;
        debugger;
        console.log("list");
    }
    EnquiriesListComponent.prototype.ngOnInit = function () {
        this.matIconRegistry.addSvgIconInNamespace("custom-svg", "angular", this.domSanitizer.bypassSecurityTrustResourceUrl("assets/images/angular_solidBlack.svg"));
        this.matIconRegistry.registerFontClassAlias("fontawesome", "fa");
    };
    EnquiriesListComponent.prototype.triggerRipple = function () {
        var point1 = this.ripple.launch(0, 0, { color: "pink", centered: true, persistent: true, radius: 50 });
        var point2 = this.ripple.launch(0, 0, { color: "yellow", centered: true, persistent: true, radius: 20 });
        setTimeout(function () {
            point1.fadeOut();
        }, 500);
    };
    EnquiriesListComponent.prototype.clearRipple = function () {
        this.ripple.fadeOutAll();
    };
    tslib_1.__decorate([
        ViewChild(MatRipple),
        tslib_1.__metadata("design:type", MatRipple)
    ], EnquiriesListComponent.prototype, "ripple", void 0);
    EnquiriesListComponent = tslib_1.__decorate([
        Component({
            selector: "enquiries-list",
            templateUrl: "./enquiries-list.component.html",
            styleUrls: ["./enquiries-list.component.css"]
        }),
        tslib_1.__metadata("design:paramtypes", [MatIconRegistry, DomSanitizer])
    ], EnquiriesListComponent);
    return EnquiriesListComponent;
}());
export { EnquiriesListComponent };
//# sourceMappingURL=enquiries-list.component.js.map