import * as tslib_1 from "tslib";
import { Component } from "@angular/core";
import { Process } from "entities/process";
import { FormBuilder } from "@angular/forms";
import { Router } from "@angular/router";
import { EnquiriesService } from "services/enquiries.service";
var EnquiriesNewComponent = /** @class */ (function () {
    function EnquiriesNewComponent(enquiriesService, router, formBuilder) {
        this.enquiriesService = enquiriesService;
        this.router = router;
        this.formBuilder = formBuilder;
        this.enquiryGeneral = formBuilder.group({
            "customerName": "",
            "contactValue": "",
            "contactPerson": "",
            "view": ""
        });
    }
    EnquiriesNewComponent.prototype.createGeneral = function (enquiryGeneral) {
        var _this = this;
        enquiryGeneral.process = new Process("952a14ad-d72d-41e8-a863-13398f29fe08", "28ae3c46-8156-4963-bbf3-4aca6317e103");
        this.enquiriesService.createGeneral(enquiryGeneral).subscribe(function (createdEnquiryGeneral) {
            _this.router.navigate(["./views/enquiries/list"]);
        });
    };
    EnquiriesNewComponent = tslib_1.__decorate([
        Component({
            selector: "enquiries-new",
            templateUrl: "enquiries-new.component.html",
            styleUrls: ["enquiries-new.component.css"],
            providers: [EnquiriesService]
        }),
        tslib_1.__metadata("design:paramtypes", [EnquiriesService,
            Router,
            FormBuilder])
    ], EnquiriesNewComponent);
    return EnquiriesNewComponent;
}());
export { EnquiriesNewComponent };
//# sourceMappingURL=enquiries-new.component.js.map