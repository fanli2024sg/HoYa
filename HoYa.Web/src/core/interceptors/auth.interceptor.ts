import { Injectable } from "@angular/core";
import {
    HttpEvent, HttpInterceptor, HttpHandler, HttpRequest
} from "@angular/common/http"; 
import { Observable } from 'rxjs';
import { AuthService } from 'core/services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(private authService: AuthService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler) {
        // Get the auth token from the service.
        //const token = this.authService.getAuthorizationToken();
        /*
        * The verbose way:
        // Clone the request and replace the original headers with
        // cloned headers, updated with the authorization.
        const authReq = req.clone({
          headers: req.headers.set("Authorization", authToken)
        });
        */
        // Clone the request and set the new header in one step.
        const authReq = req.clone({ setHeaders: { Authorization: `Bearer ${this.authService.token}` } });

        // send cloned request with header to the next handler.
        return next.handle(authReq);
    }
}
