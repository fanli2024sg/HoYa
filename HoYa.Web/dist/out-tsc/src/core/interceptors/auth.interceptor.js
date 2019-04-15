import * as tslib_1 from "tslib";
import { Injectable } from "@angular/core";
import { AuthService } from 'core/services/auth.service';
var AuthInterceptor = /** @class */ (function () {
    function AuthInterceptor(authService) {
        this.authService = authService;
    }
    AuthInterceptor.prototype.intercept = function (req, next) {
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
        var authReq = req.clone({ setHeaders: { Authorization: "Bearer " + this.authService.token } });
        // send cloned request with header to the next handler.
        return next.handle(authReq);
    };
    AuthInterceptor = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [AuthService])
    ], AuthInterceptor);
    return AuthInterceptor;
}());
export { AuthInterceptor };
//# sourceMappingURL=auth.interceptor.js.map