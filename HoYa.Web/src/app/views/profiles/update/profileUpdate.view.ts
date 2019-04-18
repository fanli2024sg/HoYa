import { Component, OnInit } from "@angular/core";
import { MatTableDataSource } from "@angular/material";
import { Router, ActivatedRoute } from "@angular/router";
import { GroupsService } from "services/groups.service";
import { ProfilesService } from "services/profiles.service";
import { ProfileGroup, Profile, Person } from "entities/person";
import { Group } from "entities/group";
import { Material } from "entities/material";
import { Observable, Subject } from "rxjs";
import { MatDialog, MatDialogRef } from "@angular/material";
import { MatSnackBar } from "@angular/material";
import { AppInterface } from "interfaces/app.interface";
@Component({
    selector: "profileUpdate",
    templateUrl: "profileUpdate.view.html",
    styleUrls: ["profileUpdate.view.css"],
    providers: [
        ProfilesService,
        GroupsService
    ]
})
export class ProfileUpdateView implements OnInit {
    groups: Group[];
    profileGroups: ProfileGroup[];
    enquiriesDataSource = new MatTableDataSource<ProfileGroup>();
    profile: Profile;
    materials: Material[];
    withRefresh = false;
    filteredMaterials$: Observable<Material[]>;
    private anyLike$ = new Subject<string>();
    deleteProfileGroupIds = new Array<string>();
    constructor(
        private matSnackBar: MatSnackBar,
        private activatedRoute: ActivatedRoute,
        public router: Router,
        public groupsService: GroupsService,
        public profilesService: ProfilesService,
        public appInterface: AppInterface
    ) {      
        this.appInterface.leftIcon$.next("arrow_back");
    }

    ngOnInit() {
        this.profilesService.find(this.activatedRoute.snapshot.paramMap.get("id")).subscribe((profile: Profile) => {
            this.profile = profile;
            this.appInterface.title$.next(this.profile.value);
            if (!this.profile.definitionId) {
                this.profile.definition = new Person();
            }
            this.groupsService.select({ ownerId: this.profile.id }, false).subscribe((groups: Group[]) => {
                this.groups = groups;
            });
        });
    }

    removeProfileGroup(id: string) {
        this.deleteProfileGroupIds.push(id);
        let enquiries = this.enquiriesDataSource.data;
        enquiries = enquiries.filter(x => x.id !== id);
        this.enquiriesDataSource.data = enquiries;
    }

    archiveProfileGroup(id: string, profileGroup: ProfileGroup): Promise<ProfileGroup> {
        return new Promise((resolve) => {
            this.profilesService.archiveGroup(id, profileGroup).subscribe(() => {
                resolve();
            });
        });
    }

    createProfileGroup(profileGroup: ProfileGroup): Promise<ProfileGroup> {
        return new Promise((resolve) => {
            this.profilesService.createGroup(profileGroup).subscribe((createdProfileGroup: ProfileGroup) => {
                resolve(createdProfileGroup);
            });
        });
    }

    updateProfile(profile: Profile): Promise<Profile> {
        return new Promise((resolve) => {
            this.profilesService.update(profile.id, profile).subscribe((updatedProfile: Profile) => {
                resolve(updatedProfile);
            });
        });
    }

    save() {
        let promises: Promise<ProfileGroup>[] = new Array<Promise<ProfileGroup>>();
        promises.push();
        this.profileGroups.filter(x => x.createdDate === null).forEach((profileGroup: ProfileGroup) => {
            if (profileGroup.createdDate) promises.push(this.archiveProfileGroup(profileGroup.id, profileGroup));
            else promises.push(this.createProfileGroup(profileGroup));
        });

        Promise.all(promises).then(() => {
            this.updateProfile(this.profile).then(() => {
                this.matSnackBar.open("儲存成功", "萬德佛!ಥ◡ಥ", {
                    duration: 5000,
                });
                this.router.navigate(["./views/enquiries/profiles"]);
            });
        });
    }
}
