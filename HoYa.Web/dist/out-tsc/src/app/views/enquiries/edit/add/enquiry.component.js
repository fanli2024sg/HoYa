import * as tslib_1 from "tslib";
import { Component, ViewChild } from "@angular/core";
import { MatRipple } from "@angular/material";
import { Router, ActivatedRoute } from "@angular/router";
import { FormBuilder } from "@angular/forms";
import { EnquiriesService } from "services/enquiries.service";
import { MaterialsService } from "services/materials.service";
import { Subject } from 'rxjs';
import { switchMap, debounceTime, distinctUntilChanged } from 'rxjs/operators';
var EnquiryComponent = /** @class */ (function () {
    function EnquiryComponent(route, router, formBuilder, enquiriesService, materialsService) {
        this.route = route;
        this.router = router;
        this.formBuilder = formBuilder;
        this.enquiriesService = enquiriesService;
        this.materialsService = materialsService;
        this.enquiries = new Array();
        this.withRefresh = false;
        this.anyLike$ = new Subject();
    }
    EnquiryComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.filteredMaterials$ = this.anyLike$.pipe(debounceTime(500), distinctUntilChanged(), switchMap(function (anyLike) { return _this.materialsService.search(anyLike, _this.withRefresh); }));
    };
    EnquiryComponent.prototype.add = function () {
    };
    tslib_1.__decorate([
        ViewChild(MatRipple),
        tslib_1.__metadata("design:type", MatRipple)
    ], EnquiryComponent.prototype, "ripple", void 0);
    EnquiryComponent = tslib_1.__decorate([
        Component({
            selector: "enquiry",
            templateUrl: "./enquiry.component.html",
            providers: [MaterialsService],
            styleUrls: ["enquiry.component.css"]
        }),
        tslib_1.__metadata("design:paramtypes", [ActivatedRoute,
            Router,
            FormBuilder,
            EnquiriesService,
            MaterialsService])
    ], EnquiryComponent);
    return EnquiryComponent;
}());
export { EnquiryComponent };
//# sourceMappingURL=enquiry.component.js.map