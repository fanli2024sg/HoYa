import * as tslib_1 from "tslib";
import { Injectable } from "@angular/core";
import { HttpService } from "core/services/http.service";
var FolderFilesService = /** @class */ (function () {
    function FolderFilesService(httpService) {
        this.httpService = httpService;
        this.api = "api/FolderFiles/";
    }
    FolderFilesService.prototype.get = function () {
        return this.httpService.get(this.api);
    };
    FolderFilesService.prototype.getByGroupId = function (groupId) {
        return this.httpService.get(this.api + "ByGroup?Id=" + groupId);
    };
    FolderFilesService.prototype.getById = function (id) {
        return this.httpService.get(this.api + id);
    };
    FolderFilesService.prototype.create = function (folderFile) {
        return this.httpService.create(this.api, folderFile);
    };
    FolderFilesService.prototype.update = function (folderFile) {
        return this.httpService.update(this.api + folderFile.id, folderFile);
    };
    FolderFilesService.prototype.delete = function (id) {
        return this.httpService.delete(this.api + id);
    };
    FolderFilesService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [HttpService])
    ], FolderFilesService);
    return FolderFilesService;
}());
export { FolderFilesService };
//# sourceMappingURL=folderFiles.service.js.map