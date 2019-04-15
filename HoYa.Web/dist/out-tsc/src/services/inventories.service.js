import * as tslib_1 from "tslib";
import { Injectable } from "@angular/core";
;
import { HttpService } from "@core/services/http.service";
var InventoriesService = /** @class */ (function () {
    function InventoriesService(httpService) {
        this.httpService = httpService;
        this.api = "api/Inventories/";
    }
    InventoriesService.prototype.get = function (x) {
        return this.httpService.get(this.api);
    };
    InventoriesService.prototype.getByProfileId = function (profileId) {
        return this.httpService.get(this.api + "ByProfile?Id=" + profileId);
    };
    InventoriesService.prototype.getById = function (id) {
        return this.httpService.get(this.api + id);
    };
    InventoriesService.prototype.create = function (inventory) {
        return this.httpService.create(this.api, inventory);
    };
    InventoriesService.prototype.update = function (inventory) {
        return this.httpService.update(this.api + inventory.id, inventory);
    };
    InventoriesService.prototype.delete = function (id) {
        return this.httpService.delete(this.api + id);
    };
    InventoriesService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [HttpService])
    ], InventoriesService);
    return InventoriesService;
}());
export { InventoriesService };
//# sourceMappingURL=inventories.service.js.map