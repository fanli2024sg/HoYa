import * as tslib_1 from "tslib";
import { Injectable } from "@angular/core";
import { HttpService } from "@core/services/http.service";
var MaterialsService = /** @class */ (function () {
    function MaterialsService(httpService) {
        this.httpService = httpService;
        this.api = "api/Materials/";
    }
    MaterialsService.prototype.get = function (x) {
        return this.httpService.get(this.api + "By?anyLike=" + x.anyLike);
    };
    MaterialsService.prototype.create = function (material) {
        return this.httpService.create(this.api, material);
    };
    MaterialsService.prototype.update = function (id, material) {
        return this.httpService.update(this.api + id, material);
    };
    MaterialsService.prototype.delete = function (id) {
        return this.httpService.delete(this.api + id);
    };
    MaterialsService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [HttpService])
    ], MaterialsService);
    return MaterialsService;
}());
export { MaterialsService };
//# sourceMappingURL=materials.service.js.map