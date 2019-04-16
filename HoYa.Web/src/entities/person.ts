import { RealSimpleGeneral, Folder, Relation, TypeDefinition } from "./entity";
import { AspNetUser } from "./identity";
import { Group } from "./group";
export class Person extends TypeDefinition {
    givenName: string;
    surName: string;
    constructor() {
        super();
    }
}

export class Profile extends RealSimpleGeneral<Person> {
    userId: string;
    user: AspNetUser;
    constructor() {
        super();
        this.definition = new Person();
    }
}
export class ProfileGroup extends Relation<Profile, Group>
{
    constructor() {
        super();
    }
}