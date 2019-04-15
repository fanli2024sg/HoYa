import * as tslib_1 from "tslib";
import { Injectable } from "@angular/core";
import * as $ from "jquery";
var HeightService = /** @class */ (function () {
    function HeightService() {
    }
    HeightService.prototype.setMobileHeight = function () {
        this.mobileHeight = ($(window).height() - 40.4) + "px";
    };
    HeightService.prototype.setMobileDiv1Height = function (add) {
        this.mobileDiv1Height = ($(window).height() - 275.4 - add) + "px";
    };
    HeightService.prototype.setMobileDiv2Height = function (add) {
        this.mobileDiv2Height = ($(window).height() - 255.4 - add) + "px";
    };
    HeightService.prototype.setTable1 = function (tfoot1Height) {
        if (!tfoot1Height)
            tfoot1Height = 0;
        this.table1Height = ($(window).height() - 289) + "px";
        this.tbody1Height = ($(window).height() - tfoot1Height - 356) + "px";
        this.removeDiv2();
    };
    HeightService.prototype.setTable2 = function () {
        this.tbody2Height = ($(window).height() - 289) + "px";
    };
    HeightService.prototype.setTable4 = function () {
        this.table4Height = ($(window).height() - 252) + "px";
        this.tbody4Height = ($(window).height() - 275) + "px";
    };
    HeightService.prototype.removeDiv2 = function () {
        this.table3Height = ($(window).height() - 252) + "px";
        this.tbody3Height = ($(window).height() - 278) + "px";
    };
    HeightService.prototype.setDiv1 = function () {
        this.div1Height = (Number(this.table1Height) + 100) + "px";
    };
    HeightService.prototype.setDiv2 = function (div2Height) {
        this.div2Height = div2Height + "px";
    };
    HeightService.prototype.setTable3 = function (tfoot3Height, ul3Height) {
        if (!tfoot3Height)
            tfoot3Height = 0;
        if (!ul3Height)
            ul3Height = 0;
        this.table3Height = (Number(this.table1Height.replace("px", "")) - Number(this.div2Height.replace("px", "")) - ul3Height - 1) + "px";
        this.tbody3Height = (Number(this.tbody1Height.replace("px", "")) - Number(this.div2Height.replace("px", "")) - tfoot3Height - 1) + "px";
        if ($(window).width() <= 768) {
            this.div2Height = "";
        }
    };
    HeightService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [])
    ], HeightService);
    return HeightService;
}());
export { HeightService };
//# sourceMappingURL=height.service.js.map