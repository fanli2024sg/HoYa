import * as tslib_1 from "tslib";
import { Injectable } from "@angular/core";
import { HttpService } from "core/services/http.service";
var PeopleService = /** @class */ (function () {
    function PeopleService(httpService) {
        this.httpService = httpService;
        this.api = "api/People/";
    }
    PeopleService.prototype.get = function () {
        return this.httpService.get(this.api);
    };
    PeopleService.prototype.getBy = function (x) {
        return this.httpService.get(this.api + "By?ParentId=" + x.parentId);
    };
    PeopleService.prototype.findByDocumentNo = function (documentNo) {
        return this.httpService.get(this.api + "By?documentNo=" + documentNo);
    };
    PeopleService.prototype.findByEmployeeNo = function (employeeNo) {
        return this.httpService.get(this.api + "By?employeeNo=" + employeeNo);
    };
    PeopleService.prototype.getOptions = function (x) {
        return this.httpService.get(this.api +
            "Option?typeId=e979aefd-385c-4445-9844-6e151ee141a1" +
            "&anyLike=" + (x.anyLike ? x.anyLike : "") +
            "&pageSize=200");
    };
    PeopleService.prototype.getById = function (id) {
        return this.httpService.get(this.api + id);
    };
    PeopleService.prototype.delete = function (id) {
        return this.httpService.delete(this.api + id);
    };
    PeopleService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [HttpService])
    ], PeopleService);
    return PeopleService;
}());
export { PeopleService };
//# sourceMappingURL=people.service.js.map