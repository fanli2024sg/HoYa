import * as tslib_1 from "tslib";
import { Component } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { EnquiriesService } from "services/enquiries.service";
var EnquiriesNewComponent = /** @class */ (function () {
    function EnquiriesNewComponent(enquiriesService, router, formBuilder) {
        this.enquiriesService = enquiriesService;
        this.router = router;
        this.formBuilder = formBuilder;
        this.enquiryGeneral = formBuilder.group({
            // 定義表格的預設值
            "value": [null, Validators.compose([Validators.required, Validators.minLength(6)])],
            "password": [null, Validators.compose([Validators.required, Validators.minLength(6)])]
        });
    }
    EnquiriesNewComponent.prototype.createGeneral = function (enquiryGeneral) {
        this.enquiriesService.createGeneral(enquiryGeneral).subscribe(function (createdEnquiryGeneral) {
            console.log(createdEnquiryGeneral);
        });
    };
    EnquiriesNewComponent = tslib_1.__decorate([
        Component({
            selector: "enquiries-new",
            templateUrl: "enquiries-new.component.html",
            styleUrls: ['enquiries-new.component.css'],
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