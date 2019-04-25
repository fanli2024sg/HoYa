import { Component, OnInit, ViewChild, ElementRef } from "@angular/core";
import { ProfilesService } from "services/profiles.service";
import { MatTableDataSource, MatPaginator, PageEvent, MatSort, Sort } from "@angular/material";
import { Profile } from "entities/person";
import { Query } from "models/query";
import { Observable } from "rxjs";
import { AppInterface } from "interfaces/app.interface";
import { debounceTime, switchMap, distinctUntilChanged } from 'rxjs/operators';
@Component({
    selector: "profiles",
    templateUrl: "profiles.component.html",
    styleUrls: ["profiles.component.css"],
    providers: [
        ProfilesService
    ]
})
export class ProfilesComponent implements OnInit {
    @ViewChild("sortTable") sortTable: MatSort;
    @ViewChild("profilesPaginator") profilesPaginator: MatPaginator;
    @ViewChild("profilesFilter") profilesFilter: ElementRef;
    anyLike: string;
    preLike: string;
    currentPage: PageEvent;
    currentSort: Sort;
    anyLike$: Observable<string> = this.appService.anyLike$;
    filteredProfiles$: Observable<Profile[]>;
    profiles = new MatTableDataSource<Profile>();
    profilesPaginatorLength: number;
    constructor(private profilesService: ProfilesService,
        private appService: AppInterface) { }
    ngOnInit() {
        this.appService.title$.next("檔案列表");
        this.appService.leftIcon$.next("menu");
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
        this.getProfiles();
        this.profilesPaginator.page.subscribe((page: PageEvent) => {
            this.currentPage = page;
            this.getProfiles();
        });




        this.anyLike$.pipe(
            debounceTime(200),
            distinctUntilChanged(),
            switchMap(anyLike => this.profilesService.filter({
                anyLike: anyLike,
                pageIndex: this.currentPage.pageIndex + 1,
                pageSize: this.currentPage.pageSize,
                sortBy: this.currentSort.active,
                orderBy: this.currentSort.direction
            }, false))
        ).subscribe((query: Query<Profile>) => {

            this.profilesPaginatorLength = query.paginatorLength;
            this.profiles.data = query.data;
        });
    }

    changeSort(sortInfo: Sort) {
        if (sortInfo.active === "ContactPerson") {
            sortInfo.active = "ContactPerson";
        }
        this.currentSort = sortInfo;
        this.getProfiles();

    }
    profileCreateDialog() {}
    getProfiles() {
        this.profilesService
            .filter({
                anyLike: this.appService.anyLike$.value,
                pageIndex: this.currentPage.pageIndex + 1,
                pageSize: this.currentPage.pageSize,
                sortBy: this.currentSort.active,
                orderBy: this.currentSort.direction
            }, false)
            .subscribe((query: Query<Profile>) => {
                this.profilesPaginatorLength = query.paginatorLength;
                this.profiles.data = query.data;

            });
    }
}