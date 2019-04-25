import { Component, OnInit, ViewChild, ElementRef } from "@angular/core";
import { InquiriesService } from "services/inquiries.service";
import { InquiryGeneralsService } from "services/inquiryGenerals.service";
import { MatTableDataSource, MatPaginator, PageEvent, MatSort, Sort, MatDialog } from "@angular/material";
import { InquiryGeneral } from "entities/inquiry";
import { Query } from "models/query";
import { AppInterface } from "interfaces/app.interface";
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { InquiryGeneralCreateDialog } from "./create/inquiryGeneralCreate.dialog";
@Component({
    selector: "inquiryGenerals",
    templateUrl: "inquiryGenerals.component.html",
    providers: [
        InquiriesService,
        InquiryGeneralsService
    ]
})
export class InquiryGeneralsComponent implements OnInit {
    @ViewChild("sortTable") sortTable: MatSort;
    @ViewChild("inquiryGeneralsPaginator") inquiryGeneralsPaginator: MatPaginator;
    @ViewChild("inquiryGeneralsFilter") inquiryGeneralsFilter: ElementRef;
    anyLike$: Observable<string> = this.appService.anyLike$;
    page$: Observable<PageEvent> = this.appService.page$;
    sort$: Observable<Sort> = this.appService.sort$;
    filteredInquiryGenerals$: Observable<InquiryGeneral[]>;
    steps = new MatTableDataSource<InquiryGeneral>();
    inquiryGenerals = new MatTableDataSource<InquiryGeneral>();
    inquiryGeneralsPaginatorLength: number;
    constructor(
        private inquiriesService: InquiriesService,
        public dialog: MatDialog,
        private inquiryGeneralsService: InquiryGeneralsService,
        public appService: AppInterface) {
    }
    ngOnInit() {
        this.appService.title$.next("詢價列表");
        this.appService.leftIcon$.next("menu");

        this.getInquiryGenerals(this.appService.anyLike$.value);
        this.inquiryGeneralsPaginator.page.subscribe((page: PageEvent) => {
            this.appService.page$.next(page);
            this.getInquiryGenerals(this.appService.anyLike$.value).subscribe((query: Query<InquiryGeneral>) => {
                this.inquiryGeneralsPaginatorLength = query.paginatorLength;
                this.inquiryGenerals.data = query.data;

            });
        });

        this.anyLike$.pipe(
            debounceTime(200),
            distinctUntilChanged(),
            switchMap((anyLike: string) => this.getInquiryGenerals(anyLike))
        ).subscribe((query: Query<InquiryGeneral>) => {

            this.inquiryGeneralsPaginatorLength = query.paginatorLength;
            this.inquiryGenerals.data = query.data;
        });
    }

    inquiryGeneralCreateDialog() {
        this.dialog.open(InquiryGeneralCreateDialog, {
            width: "500px",
            data: new InquiryGeneral("952a14ad-d72d-41e8-a863-13398f29fe08", "28ae3c46-8156-4963-bbf3-4aca6317e103")
        }).afterClosed().subscribe((inquiryGeneral: InquiryGeneral) => {
            this.getInquiryGenerals(this.appService.anyLike$.value).subscribe((query: Query<InquiryGeneral>) => {
                this.inquiryGeneralsPaginatorLength = query.paginatorLength;
                this.inquiryGenerals.data = query.data;
            });
        });
    }

    changeSort(soft: Sort) {
        if (soft.active === 'ContactPerson') {
            soft.active = 'ContactPerson';
        }
        this.appService.sort$.next(soft);
        this.getInquiryGenerals(this.appService.anyLike$.value).subscribe((query: Query<InquiryGeneral>) => {
            this.inquiryGeneralsPaginatorLength = query.paginatorLength;
            this.inquiryGenerals.data = query.data;

        });

    }

    getInquiryGenerals(anyLike: string) {

        return this.inquiryGeneralsService
            .filter({
                anyLike: anyLike,
                pageIndex: this.appService.page$.value.pageIndex + 1,
                pageSize: this.appService.page$.value.pageSize,
                sortBy: this.appService.sort$.value.active,
                orderBy: this.appService.sort$.value.direction
            }, false)
            
    }

    reply(emailRow) {
        console.log('回覆信件', emailRow);
    }

    delete(emailRow) {
        console.log('刪除信件', emailRow);
    }
}