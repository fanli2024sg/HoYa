import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ProfilesComponent } from "./profiles.component";
import { ProfileUpdateView } from "./update/profileUpdate.view";
export const profilesRoutes: Routes = [
    {
        path: "",
        component: ProfilesComponent
    },
    {
        path: ":id",
        component: ProfileUpdateView
    }
];
@NgModule({
    imports: [
        RouterModule.forChild(profilesRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class ProfilesRouting { }
