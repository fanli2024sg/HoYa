export class AspNetUser {
    id: string;
    email: string;
    password: string;
    value: string;
    roles: UserRole[];
    constructor() {
        this.roles = new Array<UserRole>();
    }
}

export class UserRole {
    userId: string;
    roleId: string;
    value: string;
    constructor(userId: string, roleId: string) {
        this.userId = userId;
        this.roleId = roleId;
    }
}

export class Role {
    id: string;
    name: string;
    description: string;
    sort: number;

    constructor() {
    }
}

