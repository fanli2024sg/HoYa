import * as tslib_1 from "tslib";
import { Injectable } from "@angular/core";
import { HttpService } from "@core/services/http.service";
var ProfilesService = /** @class */ (function () {
    function ProfilesService(httpService) {
        this.httpService = httpService;
        this.api = "api/Profiles/";
    }
    ProfilesService.prototype.get = function (x) {
        return this.httpService.get(this.api);
    };
    ProfilesService.prototype.find = function (id) {
        return this.httpService.get(this.api + id);
    };
    ProfilesService.prototype.create = function (profile) {
        return this.httpService.create(this.api, profile);
    };
    ProfilesService.prototype.update = function (id, profile) {
        return this.httpService.update(this.api + id, profile);
    };
    ProfilesService.prototype.delete = function (id) {
        return this.httpService.delete(this.api + id);
    };
    ProfilesService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [HttpService])
    ], ProfilesService);
    return ProfilesService;
}());
export { ProfilesService };
//# sourceMappingURL=profiles.service.js.map