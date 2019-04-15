import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { AuthGuard } from "@core/guards/auth.guard";
import { AuthService } from "@core/services/auth.service";
import { HttpService } from "@core/services/http.service";
import { AppRoutingModule } from "./app-routing.module";
import { ErrorStateMatcher, ShowOnDirtyErrorStateMatcher } from "@angular/material/core";
import { AppComponent } from "./app.component";
import { HttpClientModule } from "@angular/common/http";

@NgModule({
    declarations: [AppComponent],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        BrowserAnimationsModule
    ],
    providers: [
        AuthGuard,
        AuthService,
        HttpService,
        { provide: ErrorStateMatcher, useClass: ShowOnDirtyErrorStateMatcher }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
