import { Component, OnInit, Inject } from "@angular/core";
import { StepsService } from "services/steps.service";
import { WorkFlow, Step } from "entities/workflow";
import { WorkFlowsService } from "services/workflows.service";
import { Observable, Subject } from "rxjs";
import { switchMap, debounceTime, distinctUntilChanged } from "rxjs/operators";
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { MatSnackBar } from "@angular/material";
@Component({
    selector: "stepCreate",
    templateUrl: "stepCreate.dialog.html",
    styleUrls: ["stepCreate.dialog.css"],
    providers: [
        StepsService,
        WorkFlowsService
    ]
})
export class StepCreateDialog implements OnInit {
    withRefresh = false;
    filteredWorkFlows$: Observable<WorkFlow[]>;
    private anyLike$ = new Subject<string>();
    constructor(
        private matSnackBar: MatSnackBar,
        private workFlowsService: WorkFlowsService,
        private dialogRef: MatDialogRef<StepCreateDialog>,
        private stepsService: StepsService,
        @Inject(MAT_DIALOG_DATA) public step: Step
    ) {

    }

    ngOnInit() {

        this.filteredWorkFlows$ = this.anyLike$.pipe(
            debounceTime(200),
            distinctUntilChanged(),
            switchMap((anyLike: string) => this.workFlowsService.search({ anyLike: anyLike }, this.withRefresh))
        );
    }
    search(anyLike: string) {
        this.anyLike$.next(anyLike);
    }

    create() {
        this.stepsService.create(this.step).subscribe((createdStep: Step) => {
            this.matSnackBar.open(`${createdStep.value} 新增成功`, "萬德佛!ಥ◡ಥ", {
                duration: 5000,
            });
        });
    }

    selectWorkFlow(workFlow: WorkFlow) {
        this.step.owner = workFlow;
        this.step.ownerId = workFlow.id;
    }

    displayWorkFlow(workFlow?: WorkFlow): string | undefined {
        return workFlow ? workFlow.value : undefined;
    }

}
