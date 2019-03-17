import * as tslib_1 from "tslib";
import { Component } from '@angular/core';
var ContentsComponent = /** @class */ (function () {
    function ContentsComponent() {
        this.title = "";
    }
    ContentsComponent.prototype.ngOnInit = function () { };
    ContentsComponent.prototype.toggleSideNav = function (sideNav) {
        sideNav.toggle().then(function (result) {
            console.log(result);
            console.log("\uFFFD\uFFFD\u6AAC\uFFFDA\uFFFDG" + result.type);
        });
    };
    ContentsComponent.prototype.opened = function () {
        console.log("open");
    };
    ContentsComponent.prototype.closed = function () {
        console.log("close");
    };
    ContentsComponent.prototype.goto = function (title) {
        this.title = title;
    };
    ContentsComponent.prototype.back = function () {
        this.title = "";
    };
    ContentsComponent = tslib_1.__decorate([
        Component({
            selector: 'contents',
            templateUrl: './contents.component.html',
            styleUrls: ['./contents.component.css']
        }),
        tslib_1.__metadata("design:paramtypes", [])
    ], ContentsComponent);
    return ContentsComponent;
}());
export { ContentsComponent };
//# sourceMappingURL=contents.component.js.map