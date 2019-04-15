import * as tslib_1 from "tslib";
import { Injectable } from "@angular/core";
;
import { HttpService } from "core/services/http.service";
var QuotationsService = /** @class */ (function () {
    function QuotationsService(httpService) {
        this.httpService = httpService;
        this.api = "api/Quotations/";
    }
    QuotationsService.prototype.get = function (x) {
        return this.httpService.get(this.api + "By?ownerId=" + x.ownerId + "&enquiryId=" + x.enquiryId);
    };
    QuotationsService.prototype.getByTypeId = function (typeId) {
        return this.httpService.get(this.api + "ByType?Id=" + typeId);
    };
    QuotationsService.prototype.getByCodeOrValue = function (typeId, codeOrValue) {
        return this.httpService.get(this.api + "ByCodeOrValue?typeId=" + typeId + "&&like=" + codeOrValue);
    };
    QuotationsService.prototype.getById = function (id) {
        return this.httpService.get(this.api + id);
    };
    QuotationsService.prototype.create = function (quotation) {
        return this.httpService.create(this.api, quotation);
    };
    QuotationsService.prototype.createGeneral = function (quotationGeneral) {
        return this.httpService.create(this.api, quotationGeneral);
    };
    QuotationsService.prototype.updateGeneral = function (quotationGeneral) {
        return this.httpService.update(this.api + quotationGeneral.id, quotationGeneral);
    };
    QuotationsService.prototype.update = function (quotation) {
        return this.httpService.update(this.api + quotation.id, quotation);
    };
    QuotationsService.prototype.delete = function (id) {
        return this.httpService.delete(this.api + id);
    };
    QuotationsService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [HttpService])
    ], QuotationsService);
    return QuotationsService;
}());
export { QuotationsService };
//# sourceMappingURL=quotations.service.js.map