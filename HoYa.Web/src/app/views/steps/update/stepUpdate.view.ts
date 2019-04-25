import { Component, OnInit } from "@angular/core";
import { MatTableDataSource } from "@angular/material";
import { Router, ActivatedRoute } from "@angular/router";
import { GroupsService } from "services/groups.service";
import { StepsService } from "services/steps.service";
import { StepGroup, Step } from "entities/workflow";
import { Group } from "entities/group";
import { Recipe } from "entities/item";
import { Observable, Subject } from "rxjs";
import { MatDialog, MatDialogRef } from "@angular/material";
import { MatSnackBar } from "@angular/material";
import { AppInterface } from "interfaces/app.interface";
@Component({
    selector: "stepUpdate",
    templateUrl: "stepUpdate.view.html",
    styleUrls: ["stepUpdate.view.css"],
    providers: [
        StepsService,
        GroupsService
    ]
})
export class StepUpdateView implements OnInit {
    groups: Group[];
    stepGroups: StepGroup[];
    inquiriesDataSource = new MatTableDataSource<StepGroup>();
    step: Step;
    recipes: Recipe[];
    withRefresh = false;
    filteredRecipes$: Observable<Recipe[]>;
    private anyLike$ = new Subject<string>();
    deleteStepGroupIds = new Array<string>();
    constructor(
        private matSnackBar: MatSnackBar,
        private activatedRoute: ActivatedRoute,
        public router: Router,
        public groupsService: GroupsService,
        public stepsService: StepsService,
        public appInterface: AppInterface
    ) {      
        this.appInterface.leftIcon$.next("arrow_back");
    }

    ngOnInit() {
        this.stepsService.find(this.activatedRoute.snapshot.paramMap.get("id")).subscribe((step: Step) => {
            this.step = step;
            this.appInterface.title$.next(this.step.value);
           
            this.groupsService.select({ ownerId: this.step.id }, false).subscribe((groups: Group[]) => {
                this.groups = groups;
            });
        });
    }

    removeStepGroup(id: string) {
        this.deleteStepGroupIds.push(id);
        let inquiries = this.inquiriesDataSource.data;
        inquiries = inquiries.filter(x => x.id !== id);
        this.inquiriesDataSource.data = inquiries;
    }

    archiveStepGroup(id: string, stepGroup: StepGroup): Promise<StepGroup> {
        return new Promise((resolve) => {
     //       this.stepsService.archiveGroup(id, stepGroup).subscribe(() => {
         //       resolve();
           // });
        });
    }

    createStepGroup(stepGroup: StepGroup): Promise<StepGroup> {
        return new Promise((resolve) => {
          //  this.stepsService.createGroup(stepGroup).subscribe((createdStepGroup: StepGroup) => {
          //      resolve(createdStepGroup);
          //  });
        });
    }

    updateStep(step: Step): Promise<Step> {
        return new Promise((resolve) => {
            this.stepsService.update(step.id, step).subscribe((updatedStep: Step) => {
                resolve(updatedStep);
            });
        });
    }

    save() {
        let promises: Promise<StepGroup>[] = new Array<Promise<StepGroup>>();
        promises.push();
        this.stepGroups.filter(x => x.createdDate === null).forEach((stepGroup: StepGroup) => {
            if (stepGroup.createdDate) promises.push(this.archiveStepGroup(stepGroup.id, stepGroup));
            else promises.push(this.createStepGroup(stepGroup));
        });

        Promise.all(promises).then(() => {
            this.updateStep(this.step).then(() => {
                this.matSnackBar.open("儲存成功", "萬德佛!ಥ◡ಥ", {
                    duration: 5000,
                });
                this.router.navigate(["./views/inquiries/steps"]);
            });
        });
    }
}
