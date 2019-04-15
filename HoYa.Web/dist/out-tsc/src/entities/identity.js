var AspNetUser = /** @class */ (function () {
    function AspNetUser() {
        this.roles = new Array();
    }
    return AspNetUser;
}());
export { AspNetUser };
var AspNetUserRole = /** @class */ (function () {
    function AspNetUserRole(userId, roleId) {
        this.userId = userId;
        this.roleId = roleId;
    }
    return AspNetUserRole;
}());
export { AspNetUserRole };
var AspNetRole = /** @class */ (function () {
    function AspNetRole() {
    }
    return AspNetRole;
}());
export { AspNetRole };
//# sourceMappingURL=identity.js.map