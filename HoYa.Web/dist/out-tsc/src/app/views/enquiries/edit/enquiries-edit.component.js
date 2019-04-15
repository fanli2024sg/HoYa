import * as tslib_1 from "tslib";
import { Component, ViewChild } from "@angular/core";
import { MatRipple } from "@angular/material";
import { Router, ActivatedRoute } from "@angular/router";
import { FormBuilder } from "@angular/forms";
import { EnquiriesService } from "services/enquiries.service";
import { MaterialsService } from "services/materials.service";
import { Enquiry } from "entities/enquiry";
import { Subject } from 'rxjs';
import { EnquiryComponent } from './add/enquiry.component';
import { MatDialog } from '@angular/material';
var EnquiriesEditComponent = /** @class */ (function () {
    function EnquiriesEditComponent(route, router, formBuilder, enquiriesService, materialsService, dialog) {
        this.route = route;
        this.router = router;
        this.formBuilder = formBuilder;
        this.enquiriesService = enquiriesService;
        this.materialsService = materialsService;
        this.dialog = dialog;
        this.enquiries = new Array();
        this.withRefresh = false;
        this.anyLike$ = new Subject();
    }
    EnquiriesEditComponent.prototype.showAddPostDialog = function () {
        this.dialog.open(EnquiryComponent, {
            hasBackdrop: false
        });
    };
    EnquiriesEditComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.enquiries.push(new Enquiry());
        this.dialog.afterAllClosed.subscribe(function () {
            console.log('�ثe�w�g�S��dialog�F');
        });
        this.dialog.afterOpen.subscribe(function (dialogRef) {
            console.log("\uFFFDs\uFFFD\uFFFDdialog\uFFFDw\uFFFD}\uFFFD\u04A1G" + dialogRef.id);
            console.log("\uFFFD\u062Be\uFFFDw\uFFFD}\uFFFD\uFFFD " + _this.dialog.openDialogs.length + " \uFFFD\uFFFDdialog\uFFFDF");
        });
        this.enquiriesService.findGeneral(this.route.snapshot.paramMap.get("id")).subscribe(function (enquiryGeneral) {
            _this.enquiryGeneral = enquiryGeneral;
        });
    };
    EnquiriesEditComponent.prototype.search = function (anyLike) {
        this.anyLike$.next(anyLike);
    };
    tslib_1.__decorate([
        ViewChild(MatRipple),
        tslib_1.__metadata("design:type", MatRipple)
    ], EnquiriesEditComponent.prototype, "ripple", void 0);
    EnquiriesEditComponent = tslib_1.__decorate([
        Component({
            selector: "enquiries-edit",
            templateUrl: "./enquiries-edit.component.html",
            providers: [EnquiriesService, MaterialsService],
            styleUrls: ["enquiries-edit.component.css"]
        }),
        tslib_1.__metadata("design:paramtypes", [ActivatedRoute,
            Router,
            FormBuilder,
            EnquiriesService,
            MaterialsService, MatDialog])
    ], EnquiriesEditComponent);
    return EnquiriesEditComponent;
}());
export { EnquiriesEditComponent };
//# sourceMappingURL=enquiries-edit.component.js.map