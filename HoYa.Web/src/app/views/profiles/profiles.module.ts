import { NgModule } from "@angular/core";
import { ProfilesComponent } from "./profiles.component";
import { ProfileUpdateView } from "./update/profileUpdate.view";
import { ProfileCreateComponent } from "./create/profileCreate.component";
import { ProfilesRouting } from "./profiles.routing";
import { AppCommon } from "app/app.common";
@NgModule({
    imports: [       
        ProfilesRouting,
        AppCommon
    ],
    declarations: [
        ProfileCreateComponent,
        ProfilesComponent,
        ProfileUpdateView
    ],
    entryComponents: [
        ProfileCreateComponent 
    ]
})

export class ProfilesModule { }