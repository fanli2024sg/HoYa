import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { HttpService } from "services/http.service";
import { AppRouting } from "./app.routing";
import { AppComponent } from "./app.component";
import { HttpClientModule } from "@angular/common/http";
import { ServiceWorkerModule } from "@angular/service-worker";
import { environment } from "environments/environment";
import { AuthInterceptor } from "interceptors/auth.interceptor";
import { AuthService } from "services/auth.service";
import { HTTP_INTERCEPTORS } from "@angular/common/http";
@NgModule({
    declarations: [AppComponent],
    imports: [
        BrowserAnimationsModule,
        BrowserModule,
        AppRouting,
        HttpClientModule,
        ServiceWorkerModule.register("ngsw-worker.js", { enabled: environment.production })
    ],
    providers: [
        HttpService,
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
        
        AuthService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
