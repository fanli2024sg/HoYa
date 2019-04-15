import { Person, Relationship, Experience, Certificate } from "entities/person";

export class PersonOption {
    id: string;
    value: string;
    profileCardTargetNo: string;
    profileCardTargetId: string;
    phoneValue: string;
    employeeNo: string;
    employeeDepartmentName: string;
    employerShortName: string;
    employerValue: string;
    experienceEmployer: GroupOption;
    profileId: string;
    profileValue: string;
    sexValue: string;
    name: string;
    englishName: string;
    profileCardEndDate: string;
}

export class GroupOption {
    id: string;
    value: string;
    businessLicenseName: string;
    shortName: string;
    code: string;
}

export class PersonModel {
    person: Person;
    certificates: Certificate[];
    experiences: Experience[];
    histories: History[];
    relationships: Relationship[];
    savedById: string;
}
