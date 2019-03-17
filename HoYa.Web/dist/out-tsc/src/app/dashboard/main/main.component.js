import * as tslib_1 from "tslib";
import { Component, ViewChild } from '@angular/core';
import { MatIconRegistry, MatRipple } from '@angular/material';
import { DomSanitizer } from '@angular/platform-browser';
var MainComponent = /** @class */ (function () {
    function MainComponent(matIconRegistry, domSanitizer) {
        this.matIconRegistry = matIconRegistry;
        this.domSanitizer = domSanitizer;
    }
    MainComponent.prototype.ngOnInit = function () {
        this.matIconRegistry.addSvgIconInNamespace('custom-svg', 'angular', this.domSanitizer.bypassSecurityTrustResourceUrl('assets/images/angular_solidBlack.svg'));
        this.matIconRegistry.registerFontClassAlias('fontawesome', 'fa');
        this.ripple.launch(0, 0);
    };
    MainComponent.prototype.triggerRipple = function () {
        var point1 = this.ripple.launch(0, 0, { color: 'pink', centered: true, persistent: true, radius: 50 });
        var point2 = this.ripple.launch(0, 0, { color: 'yellow', centered: true, persistent: true, radius: 20 });
        setTimeout(function () {
            point1.fadeOut();
        }, 500);
    };
    MainComponent.prototype.clearRipple = function () {
        this.ripple.fadeOutAll();
    };
    tslib_1.__decorate([
        ViewChild(MatRipple),
        tslib_1.__metadata("design:type", MatRipple)
    ], MainComponent.prototype, "ripple", void 0);
    MainComponent = tslib_1.__decorate([
        Component({
            selector: 'app-main',
            templateUrl: './main.component.html',
            styleUrls: ['./main.component.css']
        }),
        tslib_1.__metadata("design:paramtypes", [MatIconRegistry, DomSanitizer])
    ], MainComponent);
    return MainComponent;
}());
export { MainComponent };
//# sourceMappingURL=main.component.js.map