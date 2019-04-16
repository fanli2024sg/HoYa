import { Component, OnInit, ViewChild, ElementRef } from "@angular/core";
import { EnquiriesService } from "services/enquiries.service";
import { MatTableDataSource, MatPaginator, PageEvent, MatSort, Sort } from "@angular/material";
import { EnquiryGeneral } from "entities/enquiry";
import { Query } from "models/query";
@Component({
    selector: "enquiryGenerals",
    templateUrl: "enquiryGenerals.component.html",
    providers: [EnquiriesService]
})
export class EnquiryGeneralsComponent implements OnInit {
    @ViewChild("sortTable") sortTable: MatSort;
    @ViewChild("enquiryGeneralsPaginator") enquiryGeneralsPaginator: MatPaginator;
    @ViewChild("enquiryGeneralsFilter") enquiryGeneralsFilter: ElementRef;
    anyLike: string;
    preLike: string;
    currentPage: PageEvent;
    currentSort: Sort;
    enquiryGenerals = new MatTableDataSource<EnquiryGeneral>();
    enquiryGeneralsPaginatorLength: number;
    constructor(private enquiriesService: EnquiriesService) { }
    ngOnInit() {
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
        this.enquiryGeneralsPaginator.page.subscribe((page: PageEvent) => {
            this.currentPage = page;
            this.getEnquiryGenerals();
        });
    }

    ngDoCheck() {
        this.anyLike = this.enquiryGeneralsFilter.nativeElement.value;
        if (this.preLike !== this.anyLike) {
            this.preLike = this.anyLike;
            this.getEnquiryGenerals();
        }
    }

    changeSort(sortInfo: Sort) {
        if (sortInfo.active === 'ContactPerson') {
            sortInfo.active = 'ContactPerson';
        }
        this.currentSort = sortInfo;
        this.getEnquiryGenerals();

    }

    getEnquiryGenerals() {

        this.enquiriesService
            .selectGeneral({
                anyLike: this.anyLike,
                pageIndex: this.currentPage.pageIndex+1,
                pageSize: this.currentPage.pageSize,
                sortBy: this.currentSort.active,
                orderBy: this.currentSort.direction
            })
            .subscribe((query: Query<EnquiryGeneral>) => {
                this.enquiryGeneralsPaginatorLength = query.paginatorLength;
                this.enquiryGenerals.data = query.data;

            });
    }

    reply(emailRow) {
        console.log('回覆信件', emailRow);
    }

    delete(emailRow) {
        console.log('刪除信件', emailRow);
    }
}