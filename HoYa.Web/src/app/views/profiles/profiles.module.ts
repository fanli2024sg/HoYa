import { NgModule } from "@angular/core";
import { ProfilesComponent } from "./profiles.component";
import { ProfilesRouting } from "./profiles.routing";
import { AppCommon } from "app/app.common";
@NgModule({
    imports: [
        ProfilesRouting,
        AppCommon
    ],
    declarations: [ProfilesComponent]
})

export class ProfilesModule { }