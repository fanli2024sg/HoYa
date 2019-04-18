import { Component, OnInit, ViewChild, ElementRef } from "@angular/core";
import { OptionsService } from "services/options.service";
import { MatTableDataSource, MatPaginator, PageEvent, MatSort, Sort } from "@angular/material";
import { Option } from "entities/entity";
import { Query } from "models/query";
@Component({
    selector: "options",
    templateUrl: "options.component.html",
    providers: [OptionsService]
})
export class OptionsComponent implements OnInit {
    @ViewChild("sortTable") sortTable: MatSort;
    @ViewChild("optionsPaginator") optionsPaginator: MatPaginator;
    @ViewChild("optionsFilter") optionsFilter: ElementRef;
    anyLike: string;
    preLike: string;
    currentPage: PageEvent;
    currentSort: Sort;
    options = new MatTableDataSource<Option>();
    optionsPaginatorLength: number;
    constructor(private optionsService: OptionsService) { }
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
        this.getOptions();
        this.optionsPaginator.page.subscribe((page: PageEvent) => {
            this.currentPage = page;
            this.getOptions();
        });
    }

    ngDoCheck() {
        this.anyLike = this.optionsFilter.nativeElement.value;
        if (this.preLike !== this.anyLike) {
            this.preLike = this.anyLike;
            this.getOptions();
        }
    }

    changeSort(sortInfo: Sort) {
        if (sortInfo.active === 'ContactPerson') {
            sortInfo.active = 'ContactPerson';
        }
        this.currentSort = sortInfo;
        this.getOptions();

    }

    getOptions() {

        this.optionsService
            .select({
                anyLike: this.anyLike,
                pageIndex: this.currentPage.pageIndex+1,
                pageSize: this.currentPage.pageSize,
                sortBy: this.currentSort.active,
                orderBy: this.currentSort.direction
            }, false)
            .subscribe((query: Query<Option>) => {
                this.optionsPaginatorLength = query.paginatorLength;
                this.options.data = query.data;

            });
    }

    reply(emailRow) {
        console.log('回覆信件', emailRow);
    }

    delete(emailRow) {
        console.log('刪除信件', emailRow);
    }
}