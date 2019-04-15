import { FlexLayoutModule } from "@angular/flex-layout";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { HttpService } from "core/services/http.service";
import { AppRoutingModule } from "./app-routing.module";
//import { ErrorStateMatcher, ShowOnDirtyErrorStateMatcher } from "@angular/material/core";
import { AppComponent } from "./app.component";
import { HttpClientModule } from "@angular/common/http";
import { ServiceWorkerModule } from "@angular/service-worker";
import { environment } from "../environments/environment";
import { httpInterceptorProviders } from "core/interceptors/index";
import { AuthService } from "core/services/auth.service";
@NgModule({
    declarations: [AppComponent],
    imports: [
        BrowserAnimationsModule,
        FlexLayoutModule,
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        ServiceWorkerModule.register("ngsw-worker.js", { enabled: environment.production })
    ],
    providers: [ 
        HttpService,
        //{ provide: ErrorStateMatcher, useClass: ShowOnDirtyErrorStateMatcher },
        httpInterceptorProviders,
        AuthService 
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
