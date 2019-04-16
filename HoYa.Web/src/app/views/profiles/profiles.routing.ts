import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ProfilesComponent } from "./profiles.component";

export const profilesRoutes: Routes = [
    {
        path: "",
        component: ProfilesComponent,
        children: []
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
