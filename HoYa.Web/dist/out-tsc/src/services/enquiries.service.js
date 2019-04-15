import * as tslib_1 from "tslib";
import { Injectable } from "@angular/core";
import { HttpService } from "core/services/http.service";
var EnquiriesService = /** @class */ (function () {
    function EnquiriesService(httpService) {
        this.httpService = httpService;
        this.api = "api/Enquiries";
    }
    EnquiriesService.prototype.get = function (x) {
        return this.httpService.get(this.api + "By?OwnerId=" + (x.ownerId || ""));
    };
    EnquiriesService.prototype.getOptions = function (x) {
        return this.httpService.get(this.api +
            "Option?typeId=e979aefd-385c-4445-9844-6e151ee141a1" +
            "&anyLike=" + (x.anyLike ? x.anyLike : "") +
            "&pageSize=200");
    };
    EnquiriesService.prototype.findGeneral = function (id) {
        return this.httpService.get(this.api + "/General/" + id);
    };
    EnquiriesService.prototype.create = function (enquiry) {
        return this.httpService.create(this.api, enquiry);
    };
    EnquiriesService.prototype.updateGeneral = function (id, enquiryGeneral) {
        return this.httpService.update(this.api + "/General/" + id, enquiryGeneral);
    };
    EnquiriesService.prototype.createGeneral = function (enquiryGeneral) {
        return this.httpService.create(this.api + "/General", enquiryGeneral);
    };
    EnquiriesService.prototype.selectGeneral = function (x) {
        return this.httpService.search(this.api + "/General", x, false);
    };
    EnquiriesService.prototype.update = function (id, enquiry) {
        return this.httpService.update(this.api + id, enquiry);
    };
    EnquiriesService.prototype.delete = function (id) {
        return this.httpService.delete(this.api + id);
    };
    EnquiriesService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [HttpService])
    ], EnquiriesService);
    return EnquiriesService;
}());
export { EnquiriesService };
//# sourceMappingURL=enquiries.service.js.map