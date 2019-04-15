import * as tslib_1 from "tslib";
import { Injectable } from "@angular/core";
import { HttpService } from "core/services/http.service";
var FoldersService = /** @class */ (function () {
    function FoldersService(httpService) {
        this.httpService = httpService;
        this.api = "api/Folders/";
    }
    FoldersService.prototype.select = function (x) {
        if (x)
            return this.httpService.get(this.api +
                ("ByGroup?Id=" + x.groupId).replace("undefine", ""));
        else
            return this.httpService.get(this.api);
    };
    FoldersService.prototype.find = function (id) {
        return this.httpService.get(this.api + id);
    };
    FoldersService.prototype.create = function (folder) {
        return this.httpService.create(this.api, folder);
    };
    FoldersService.prototype.update = function (folder) {
        return this.httpService.update(this.api + folder.id, folder);
    };
    FoldersService.prototype.delete = function (id) {
        return this.httpService.delete(this.api + id);
    };
    FoldersService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [HttpService])
    ], FoldersService);
    return FoldersService;
}());
export { FoldersService };
//# sourceMappingURL=folders.service.js.map