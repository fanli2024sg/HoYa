import { Component, OnInit, ViewChild, ElementRef } from "@angular/core";
import { StepsService } from "services/steps.service";
import { MatTableDataSource, MatPaginator, PageEvent, MatSort, Sort } from "@angular/material";
import { Step } from "entities/workflow";
import { Query } from "models/query";
import { Observable } from "rxjs";
import { AppInterface } from "interfaces/app.interface";
import { MatDialog, MatDialogRef } from "@angular/material";
import { StepCreateDialog } from "./create/stepCreate.dialog";
import { debounceTime, switchMap, distinctUntilChanged } from 'rxjs/operators';
@Component({
    selector: "steps",
    templateUrl: "steps.component.html",
    styleUrls: ["steps.component.css"],
    providers: [
        StepsService
    ]
})
export class StepsComponent implements OnInit {
    @ViewChild("sortTable") sortTable: MatSort;
    @ViewChild("stepsPaginator") stepsPaginator: MatPaginator;
    @ViewChild("stepsFilter") stepsFilter: ElementRef;
    anyLike$: Observable<string> = this.appService.anyLike$;
    page$: Observable<PageEvent> = this.appService.page$;
    sort$: Observable<Sort> = this.appService.sort$;
    filteredSteps$: Observable<Step[]>;
    steps = new MatTableDataSource<Step>();
    stepsPaginatorLength: number;
    constructor(
        private stepsService: StepsService,
        public dialog: MatDialog,
        private appService: AppInterface
    ) {

    }
    ngOnInit() {
        this.appService.title$.next("任務列表");
        this.appService.leftIcon$.next("menu");
        this.getSteps();
        this.stepsPaginator.page.subscribe((page: PageEvent) => {
            this.appService.page$.next(page);
            this.getSteps();
        });

        this.dialog.afterAllClosed.subscribe(() => {
            console.log("no dialog");
        });

        this.dialog.afterOpen.subscribe((dialogRef: MatDialogRef<any>) => {
            console.log(`new dialog:${dialogRef.id}`);
            console.log(`has ${this.dialog.openDialogs.length} dialogs`);
        });


        this.anyLike$.pipe(
            debounceTime(200),
            distinctUntilChanged(),
            switchMap((anyLike: string) => this.stepsService.search({
                anyLike: anyLike,
                pageIndex: this.appService.page$.getValue().pageIndex + 1,
                pageSize: this.appService.page$.getValue().pageSize,
                sortBy: this.appService.sort$.getValue().active,
                orderBy: this.appService.sort$.getValue().direction
            }, false))
        ).subscribe((query: Query<Step>) => {

            this.stepsPaginatorLength = query.paginatorLength;
            this.steps.data = query.data;
        });
    }

    stepCreateDialog() {
        let step = new Step();

        this.dialog.open(StepCreateDialog, {
            width: "250px",
            data: step
        }).afterClosed().subscribe((step: Step) => {
            console.log(step);
        });


    }


    changeSort(soft: Sort) {
        if (soft.active === "ContactPerson") {
            soft.active = "ContactPerson";
        }
        this.appService.sort$.next(soft);
        this.getSteps();

    }

    getSteps() {
        this.stepsService
            .select({
                anyLike: this.appService.anyLike$.getValue(),
                pageIndex: this.appService.page$.getValue().pageIndex + 1,
                pageSize: this.appService.page$.getValue().pageSize,
                sortBy: this.appService.sort$.getValue().active,
                orderBy: this.appService.sort$.getValue().direction
            })
            .subscribe((query: Query<Step>) => {
                this.stepsPaginatorLength = query.paginatorLength;
                this.steps.data = query.data;
            });
    }
}