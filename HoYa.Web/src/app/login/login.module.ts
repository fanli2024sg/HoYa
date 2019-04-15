import { NgModule } from "@angular/core";
import { LoginComponent } from "./login.component";
import { ReactiveFormsModule } from "@angular/forms";
import { LoginRoutingModule } from "./login-routing.module";
import { CoreModule } from "core/core.module";
@NgModule({    
    imports: [
        ReactiveFormsModule,
        LoginRoutingModule,
        CoreModule
    ],
    declarations: [LoginComponent]
})
export class LoginModule { }
