import * as tslib_1 from "tslib";
import { Injectable } from "@angular/core";
import { HttpService } from "@core/services/http.service";
var SettingsService = /** @class */ (function () {
    function SettingsService(httpService) {
        this.httpService = httpService;
        this.api = "api/Settings/";
    }
    SettingsService.prototype.find = function () {
        return this.httpService.get(this.api + "Settings");
    };
    SettingsService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [HttpService])
    ], SettingsService);
    return SettingsService;
}());
export { SettingsService };
//# sourceMappingURL=settings.service.js.map