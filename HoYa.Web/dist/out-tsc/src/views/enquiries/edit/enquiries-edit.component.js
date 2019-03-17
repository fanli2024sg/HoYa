import * as tslib_1 from "tslib";
import { Component, ViewChild } from "@angular/core";
import { MatIconRegistry, MatRipple } from "@angular/material";
import { DomSanitizer } from "@angular/platform-browser";
import { Router, ActivatedRoute } from "@angular/router";
import { EnquiriesService } from "services/enquiries.service";
var EnquiriesEditComponent = /** @class */ (function () {
    function EnquiriesEditComponent(route, router, matIconRegistry, domSanitizer, enquiriesService) {
        this.route = route;
        this.router = router;
        this.matIconRegistry = matIconRegistry;
        this.domSanitizer = domSanitizer;
        this.enquiriesService = enquiriesService;
        debugger;
        console.log("id:");
        console.log(this.route.snapshot.paramMap.get("id"));
    }
    EnquiriesEditComponent.prototype.ngOnInit = function () {
        console.log(this.route.snapshot.paramMap.get("id"));
        this.matIconRegistry.addSvgIconInNamespace("custom-svg", "angular", this.domSanitizer.bypassSecurityTrustResourceUrl("assets/images/angular_solidBlack.svg"));
        this.matIconRegistry.registerFontClassAlias("fontawesome", "fa");
    };
    EnquiriesEditComponent.prototype.triggerRipple = function () {
        var point1 = this.ripple.launch(0, 0, { color: "pink", centered: true, persistent: true, radius: 50 });
        var point2 = this.ripple.launch(0, 0, { color: "yellow", centered: true, persistent: true, radius: 20 });
        setTimeout(function () {
            point1.fadeOut();
        }, 500);
    };
    EnquiriesEditComponent.prototype.clearRipple = function () {
        this.ripple.fadeOutAll();
    };
    tslib_1.__decorate([
        ViewChild(MatRipple),
        tslib_1.__metadata("design:type", MatRipple)
    ], EnquiriesEditComponent.prototype, "ripple", void 0);
    EnquiriesEditComponent = tslib_1.__decorate([
        Component({
            selector: "enquiries-edit",
            templateUrl: "./enquiries-edit.component.html",
            providers: [EnquiriesService],
            styleUrls: ["./enquiries-edit.component.css"]
        }),
        tslib_1.__metadata("design:paramtypes", [ActivatedRoute,
            Router,
            MatIconRegistry,
            DomSanitizer,
            EnquiriesService])
    ], EnquiriesEditComponent);
    return EnquiriesEditComponent;
}());
export { EnquiriesEditComponent };
//# sourceMappingURL=enquiries-edit.component.js.map