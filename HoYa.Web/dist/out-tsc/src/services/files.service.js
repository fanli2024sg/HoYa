import * as tslib_1 from "tslib";
import { Injectable } from "@angular/core";
import { HttpService } from "core/services/http.service";
var FilesService = /** @class */ (function () {
    function FilesService(httpService) {
        this.httpService = httpService;
        this.api = "api/Files/";
    }
    FilesService.prototype.get = function () {
        return this.httpService.get(this.api);
    };
    FilesService.prototype.getByGroupId = function (groupId) {
        return this.httpService.get(this.api + "ByGroup?Id=" + groupId);
    };
    FilesService.prototype.getById = function (id) {
        return this.httpService.get(this.api + id);
    };
    FilesService.prototype.create = function (files) {
        return this.httpService.upload(this.api, files);
    };
    FilesService.prototype.update = function (file) {
        return this.httpService.update(this.api + file.id, file);
    };
    FilesService.prototype.delete = function (id) {
        return this.httpService.delete(this.api + id);
    };
    FilesService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [HttpService])
    ], FilesService);
    return FilesService;
}());
export { FilesService };
//# sourceMappingURL=files.service.js.map