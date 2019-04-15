import { Entity, Folder, Relation } from "./entity";
import { AspNetUser } from "./identity";
import { Group } from "./group";
export class Person extends Entity {
    profileId: string;
    profile: Profile;
    constructor() {
        super();
    }
}

export class Profile extends Entity {
    userId: string;
    user: AspNetUser;
    value: string;
    no: string;
    galleryId: string;
    gallery: Folder;
    constructor() {
        super();
    }
}

export class ProfileGroup extends Relation<Profile, Group>
{
    constructor() {
        super();
    }
}