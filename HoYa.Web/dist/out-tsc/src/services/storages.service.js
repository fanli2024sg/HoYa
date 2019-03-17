import * as tslib_1 from "tslib";
import { Injectable } from "@angular/core";
;
import { HttpService } from "@core/services/http.service";
var StoragesService = /** @class */ (function () {
    function StoragesService(httpService) {
        this.httpService = httpService;
        this.api = "api/Storages/";
    }
    StoragesService.prototype.get = function (x) {
        return this.httpService.get(this.api + "By?anyLike=" + (x.anyLike || ""));
    };
    StoragesService.prototype.getById = function (id) {
        return this.httpService.get(this.api + id);
    };
    StoragesService.prototype.create = function (storage) {
        return this.httpService.create(this.api, storage);
    };
    StoragesService.prototype.update = function (storage) {
        return this.httpService.update(this.api + storage.id, storage);
    };
    StoragesService.prototype.delete = function (id) {
        return this.httpService.delete(this.api + id);
    };
    StoragesService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [HttpService])
    ], StoragesService);
    return StoragesService;
}());
export { StoragesService };
//# sourceMappingURL=storages.service.js.map