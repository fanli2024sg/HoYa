import * as tslib_1 from "tslib";
import { Injectable } from "@angular/core";
import { HttpService } from "@core/services/http.service";
var MaterialProceduresService = /** @class */ (function () {
    function MaterialProceduresService(httpService) {
        this.httpService = httpService;
        this.api = "api/MaterialProcedures/";
    }
    MaterialProceduresService.prototype.get = function (x) {
        return this.httpService.get(this.api + "By?anyLike=" + x.anyLike);
    };
    MaterialProceduresService.prototype.create = function (materialProcedure) {
        return this.httpService.create(this.api, materialProcedure);
    };
    MaterialProceduresService.prototype.update = function (id, materialProcedure) {
        return this.httpService.update(this.api + id, materialProcedure);
    };
    MaterialProceduresService.prototype.delete = function (id) {
        return this.httpService.delete(this.api + id);
    };
    MaterialProceduresService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [HttpService])
    ], MaterialProceduresService);
    return MaterialProceduresService;
}());
export { MaterialProceduresService };
//# sourceMappingURL=materialprocedures.service.js.map