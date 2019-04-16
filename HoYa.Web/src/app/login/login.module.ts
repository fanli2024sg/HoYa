import { NgModule } from "@angular/core";
import { LoginComponent } from "./login.component";
import { ReactiveFormsModule } from "@angular/forms";
import { LoginRouting } from "./login.routing";
import { AppCommon } from "app/app.common";
@NgModule({    
    imports: [
        ReactiveFormsModule,
        LoginRouting,
        AppCommon
    ],
    declarations: [LoginComponent]
})
export class LoginModule { }
