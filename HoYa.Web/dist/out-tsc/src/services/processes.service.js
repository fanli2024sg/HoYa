import * as tslib_1 from "tslib";
import { Injectable } from "@angular/core";
import { HttpService } from "core/services/http.service";
var ProcessesService = /** @class */ (function () {
    function ProcessesService(httpService) {
        this.httpService = httpService;
        this.api = "api/Processes/";
    }
    ProcessesService.prototype.get = function (x) {
        return this.httpService.get(this.api);
    };
    ProcessesService.prototype.getNo = function (x) {
        return this.httpService.get(this.api + "No?typeId=" + x.typeId);
    };
    ProcessesService.prototype.find = function (id) {
        return this.httpService.get(this.api + id);
    };
    ProcessesService.prototype.create = function (process) {
        return this.httpService.create(this.api, process);
    };
    ProcessesService.prototype.update = function (id, process) {
        return this.httpService.update(this.api + id, process);
    };
    ProcessesService.prototype.delete = function (id) {
        return this.httpService.delete(this.api + id);
    };
    ProcessesService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [HttpService])
    ], ProcessesService);
    return ProcessesService;
}());
export { ProcessesService };
//# sourceMappingURL=processes.service.js.map