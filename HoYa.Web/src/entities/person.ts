import { RealTypeSimpleInstance, Relation, TypeSimpleDetail, Definition, Branch, Change } from "./entity";
import { AspNetUser } from "./identity";
import { Activity } from "./workflow";
import { Group } from "./group";
export class PersonDefinition extends Definition {
    constructor() {
        super();
    }
}
export class Person extends Branch<PersonDefinition>
{
    givenName: string;
    surName: string;
    constructor() {
        super();
    }
}
export class PersonChange extends Change<Person>
{
    constructor() {
        super();
    }
}
export class Contact extends TypeSimpleDetail<Person>
{
    constructor() {
        super();
    }
}
export class Profile extends RealTypeSimpleInstance<Person, PersonChange> {
    userId: string;
    user: AspNetUser;
    constructor() {
        super();
        this.definitionBranch = new Person();
    }
}
export class ProfileGroup extends Relation<Profile, Group>
{
    constructor(ownerId?: string, targetId?: string) {
        super();
        this.ownerId = ownerId;
        this.targetId = targetId;
    }
}
export class ProfileActivity extends Relation<Profile, Activity>
{
}

 