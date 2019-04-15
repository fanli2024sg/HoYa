import * as tslib_1 from "tslib";
import { Injectable } from "@angular/core";
import { HttpService } from "@core/services/http.service";
var OptionsService = /** @class */ (function () {
    function OptionsService(httpService) {
        this.httpService = httpService;
        this.api = "api/Options/";
    }
    OptionsService.prototype.get = function (x) {
        return this.httpService.get(this.api + "By?parentId=" + x.parentId +
            "&anyLike=" + x.anyLike);
    };
    OptionsService.prototype.getByGroupValue = function (groupValue) {
        return this.httpService.get(this.api + "ByGroupValue?Value=" + groupValue);
    };
    OptionsService.prototype.find = function (id) {
        return this.httpService.get(this.api + id);
    };
    OptionsService.prototype.create = function (option) {
        return this.httpService.create(this.api, option);
    };
    OptionsService.prototype.update = function (option) {
        return this.httpService.update(this.api + option.id, option);
    };
    OptionsService.prototype.delete = function (id) {
        return this.httpService.delete(this.api + id);
    };
    OptionsService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [HttpService])
    ], OptionsService);
    return OptionsService;
}());
export { OptionsService };
//# sourceMappingURL=options.service.js.map