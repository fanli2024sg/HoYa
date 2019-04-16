import { Component, OnInit, Inject } from "@angular/core";
import { ProfilesService } from "services/profiles.service";
import { Profile } from "entities/person";
import { Observable, Subject } from "rxjs";
import { switchMap, debounceTime, distinctUntilChanged } from "rxjs/operators";
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { MatSnackBar } from "@angular/material";
@Component({
    selector: "profileCreate",
    templateUrl: "profileCreate.component.html",
    styleUrls: ["profileCreate.component.css"],
    providers: [
        ProfilesService
    ]
})
export class ProfileCreateComponent {
    withRefresh = false;
    filteredProfiles$: Observable<Profile[]>;
    private anyLike$ = new Subject<string>();
    constructor(
        private matSnackBar: MatSnackBar,
        private dialogRef: MatDialogRef<ProfileCreateComponent>,
        private profilesService: ProfilesService,
        @Inject(MAT_DIALOG_DATA) public profile: Profile
    ) {

    }
    create() {
        this.profilesService.create(this.profile).subscribe((createdProfile: Profile) => {


            this.matSnackBar.open(`${createdProfile.definition.surName}${createdProfile.definition.givenName}新增成功`, "萬德佛!ಥ◡ಥ", {
                duration: 5000,
            });
        }
        );
    }

}
