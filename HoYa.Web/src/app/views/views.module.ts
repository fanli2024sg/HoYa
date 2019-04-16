import { NgModule } from "@angular/core";
import { ViewsComponent } from "./views.component";
import { ViewsRouting } from "./views.routing";

import { AppCommon } from "app/app.common";
import { AuthGuard } from "guards/auth.guard";
@NgModule({
    imports: [
        AppCommon,
        ViewsRouting
    ],
    declarations: [ViewsComponent], providers: [AuthGuard]
})

export class ViewsModule { }