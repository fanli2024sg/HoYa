import { Component, OnInit, Inject } from "@angular/core";
import { ProfilesService } from "services/profiles.service";
import { Profile } from "entities/person";
import { Observable, Subject } from "rxjs";
import { switchMap, debounceTime, distinctUntilChanged } from "rxjs/operators";
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { MatSnackBar } from "@angular/material";
@Component({
    selector: "profileAppend",
    templateUrl: "profileAppend.component.html",
    styleUrls: ["profileAppend.component.css"],
    providers: [
        ProfilesService
    ]
})
export class ProfileAppendComponent {
    withRefresh = false;
    filteredProfiles$: Observable<Profile[]>;
    private anyLike$ = new Subject<string>();
    constructor(
        private matSnackBar: MatSnackBar,
        private dialogRef: MatDialogRef<ProfileAppendComponent>,
        private profilesService: ProfilesService,
        @Inject(MAT_DIALOG_DATA) public profile: Profile
    ) {

    }
   

}
