import * as tslib_1 from "tslib";
import { Component, ViewChild, ElementRef } from "@angular/core";
import { EnquiriesService } from "services/enquiries.service";
import { MatTableDataSource, MatPaginator, MatSort } from "@angular/material";
var EnquiriesListComponent = /** @class */ (function () {
    function EnquiriesListComponent(enquiriesService) {
        this.enquiriesService = enquiriesService;
        this.enquiryGenerals = new MatTableDataSource();
    }
    EnquiriesListComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.anyLike = "";
        this.currentPage = {
            pageIndex: 0,
            pageSize: 10,
            length: null
        };
        this.currentSort = {
            active: "",
            direction: ""
        };
        this.getEnquiryGenerals();
        this.enquiryGeneralsPaginator.page.subscribe(function (page) {
            _this.currentPage = page;
            _this.getEnquiryGenerals();
        });
    };
    EnquiriesListComponent.prototype.ngDoCheck = function () {
        this.anyLike = this.enquiryGeneralsFilter.nativeElement.value;
        if (this.preLike !== this.anyLike) {
            this.preLike = this.anyLike;
            this.getEnquiryGenerals();
        }
    };
    EnquiriesListComponent.prototype.changeSort = function (sortInfo) {
        if (sortInfo.active === 'ContactPerson') {
            sortInfo.active = 'ContactPerson';
        }
        this.currentSort = sortInfo;
        this.getEnquiryGenerals();
    };
    EnquiriesListComponent.prototype.getEnquiryGenerals = function () {
        var _this = this;
        this.enquiriesService
            .selectGeneral({
            anyLike: this.anyLike,
            pageIndex: this.currentPage.pageIndex + 1,
            pageSize: this.currentPage.pageSize,
            sortBy: this.currentSort.active,
            orderBy: this.currentSort.direction
        })
            .subscribe(function (query) {
            _this.enquiryGeneralsPaginatorLength = query.paginatorLength;
            _this.enquiryGenerals.data = query.data;
        });
    };
    EnquiriesListComponent.prototype.reply = function (emailRow) {
        console.log('�^�ЫH��', emailRow);
    };
    EnquiriesListComponent.prototype.delete = function (emailRow) {
        console.log('�R���H��', emailRow);
    };
    tslib_1.__decorate([
        ViewChild("sortTable"),
        tslib_1.__metadata("design:type", MatSort)
    ], EnquiriesListComponent.prototype, "sortTable", void 0);
    tslib_1.__decorate([
        ViewChild("enquiryGeneralsPaginator"),
        tslib_1.__metadata("design:type", MatPaginator)
    ], EnquiriesListComponent.prototype, "enquiryGeneralsPaginator", void 0);
    tslib_1.__decorate([
        ViewChild("enquiryGeneralsFilter"),
        tslib_1.__metadata("design:type", ElementRef)
    ], EnquiriesListComponent.prototype, "enquiryGeneralsFilter", void 0);
    EnquiriesListComponent = tslib_1.__decorate([
        Component({
            selector: "enquiries-list",
            templateUrl: "./enquiries-list.component.html",
            providers: [EnquiriesService]
        }),
        tslib_1.__metadata("design:paramtypes", [EnquiriesService])
    ], EnquiriesListComponent);
    return EnquiriesListComponent;
}());
export { EnquiriesListComponent };
//# sourceMappingURL=enquiries-list.component.js.map