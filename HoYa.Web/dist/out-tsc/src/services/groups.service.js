import * as tslib_1 from "tslib";
import { Injectable } from "@angular/core";
import { HttpService } from "core/services/http.service";
var GroupsService = /** @class */ (function () {
    function GroupsService(httpService) {
        this.httpService = httpService;
        this.api = "api/Groups/";
    }
    GroupsService.prototype.get = function (x) {
        return this.httpService.get(this.api);
    };
    GroupsService.prototype.find = function (id) {
        return this.httpService.get(this.api + id);
    };
    GroupsService.prototype.create = function (group) {
        return this.httpService.create(this.api, group);
    };
    GroupsService.prototype.update = function (id, group) {
        return this.httpService.update(this.api + id, group);
    };
    GroupsService.prototype.delete = function (id) {
        return this.httpService.delete(this.api + id);
    };
    GroupsService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [HttpService])
    ], GroupsService);
    return GroupsService;
}());
export { GroupsService };
//# sourceMappingURL=groups.service.js.map