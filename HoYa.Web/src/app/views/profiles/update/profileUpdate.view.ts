import { Component, OnInit } from "@angular/core";
import { MatTableDataSource } from "@angular/material";
import { Router, ActivatedRoute } from "@angular/router";
import { GroupsService } from "services/groups.service";
import { ProfilesService } from "services/profiles.service";
import { OptionsService } from "services/options.service";
import { PeopleService } from "services/people.service";
import { ProfileGroup, Profile, Person } from "entities/person";
import { Group } from "entities/group";
import { Recipe } from "entities/item";
import { Observable, Subject } from "rxjs";
import { Option } from "entities/entity";
import { MatSnackBar } from "@angular/material";
import { AppInterface } from "interfaces/app.interface";
@Component({
    selector: "profileUpdate",
    templateUrl: "profileUpdate.view.html",
    styleUrls: ["profileUpdate.view.css"],
    providers: [
        ProfilesService,
        GroupsService,
        OptionsService,
        PeopleService
    ]
})
export class ProfileUpdateView implements OnInit {
    groups: Group[];
    types$: Observable<Option[]> = this.optionsService.select({ parentId: "24741b4f-e765-40d0-a1e8-15ab515972cd" }, false);

    profileGroups: ProfileGroup[];
    inquiriesDataSource = new MatTableDataSource<ProfileGroup>();
    profile: Profile;
    recipes: Recipe[];
    withRefresh = false;
    filteredRecipes$: Observable<Recipe[]>;
    private anyLike$ = new Subject<string>();
    deleteProfileGroupIds = new Array<string>();
    constructor(
        private matSnackBar: MatSnackBar,
        private activatedRoute: ActivatedRoute,
        public router: Router,
        public groupsService: GroupsService,
        public profilesService: ProfilesService,
        public optionsService: OptionsService,
        public peopleService: PeopleService,
        public appInterface: AppInterface
    ) {      
       
    }

    ngOnInit() {
        this.appInterface.leftIcon$.next("arrow_back");
        this.profilesService.find(this.activatedRoute.snapshot.paramMap.get("id")).subscribe((profile: Profile) => {
            this.profile = profile;
            if (!this.profile.definitionBranchId) {
                this.profile.definitionBranch = new Person();
            }
            console.log(this.profile);
            this.appInterface.title$.next(`${this.profile.definitionBranch.value}(${this.profile.value})`.replace("()",""));

           // this.groups$ = this.groupsService.select({ ownerId: this.profile.id }, false);
            this.groupsService.select({ ownerId: this.profile.id }, false).subscribe((groups: Group[]) => {
                this.groups = groups;
                this.profilesService.selectGroups({ ownerId: this.profile.id }, false).subscribe((profileGroups: ProfileGroup[]) => {

                    this.profileGroups = profileGroups;
                    this.profileGroups.forEach((profileGroup: ProfileGroup) => {
                        this.groups.find(x => x.id === profileGroup.targetId)._checked = true;
                    });
                });
            });

           
        });
    }

    removeProfileGroup(id: string) {
        this.deleteProfileGroupIds.push(id);
        let inquiries = this.inquiriesDataSource.data;
        inquiries = inquiries.filter(x => x.id !== id);
        this.inquiriesDataSource.data = inquiries;
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
        console.log(this.groups);
        
        let promises: Promise<ProfileGroup>[] = new Array<Promise<ProfileGroup>>();

        this.groups.forEach((group: Group) => {
            if (group._checked) {
                if (this.profileGroups.filter(x => x.targetId === group.id).length === 0) {
                    let profileGroup = new ProfileGroup(this.profile.id, group.id); 
                    promises.push(this.createProfileGroup(profileGroup));
                }
            } else {
                if (this.profileGroups.filter(x => x.targetId === group.id && x.createdDate!==null).length === 1) {
                    let profileGroup = this.profileGroups.find(x => x.targetId === group.id);
                    profileGroup.endDate = new Date();
                    profileGroup.archivedDate = new Date();
                    promises.push(this.archiveProfileGroup(profileGroup.id, profileGroup));
                }
            }
        })
        /*
        this.profileGroups.filter(x => x.createdDate === null).forEach((profileGroup: ProfileGroup) => {
            if (profileGroup.createdDate) promises.push(this.archiveProfileGroup(profileGroup.id, profileGroup));
            else promises.push(this.createProfileGroup(profileGroup));
        });*/

        Promise.all(promises).then(() => {
            if (this.profile.definitionBranchId === null) {

                this.peopleService.create(this.profile.definitionBranch).subscribe((createdPerson: Person) => {
                    this.profile.definitionBranchId = createdPerson.id;
                    this.updateProfile(this.profile).then(() => {
                        this.matSnackBar.open("儲存成功", "萬德佛!ಥ◡ಥ", {
                            duration: 5000,
                        });
                        this.router.navigate(["./views/profiles"]);
                    });
                });
            } else {
                this.peopleService.update(this.profile.definitionBranchId, this.profile.definitionBranch).subscribe((updatedPerson: Person) => {
                    this.updateProfile(this.profile).then(() => {
                        this.matSnackBar.open("儲存成功", "萬德佛!ಥ◡ಥ", {
                            duration: 5000,
                        });
                        this.router.navigate(["./views/profiles"]);
                    });
                });
            }
        });
    }
}
