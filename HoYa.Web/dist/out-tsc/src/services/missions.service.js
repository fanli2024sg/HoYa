import * as tslib_1 from "tslib";
import { Injectable } from "@angular/core";
import { HttpService } from "@core/services/http.service";
var MissionsService = /** @class */ (function () {
    function MissionsService(httpService) {
        this.httpService = httpService;
        this.api = "api/Missions/";
    }
    MissionsService.prototype.get = function (x) {
        return this.httpService.get(this.api);
    };
    MissionsService.prototype.find = function (id) {
        return this.httpService.get(this.api + id);
    };
    MissionsService.prototype.create = function (mission) {
        return this.httpService.create(this.api, mission);
    };
    MissionsService.prototype.update = function (id, mission) {
        return this.httpService.update(this.api + id, mission);
    };
    MissionsService.prototype.delete = function (id) {
        return this.httpService.delete(this.api + id);
    };
    MissionsService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [HttpService])
    ], MissionsService);
    return MissionsService;
}());
export { MissionsService };
//# sourceMappingURL=missions.service.js.map