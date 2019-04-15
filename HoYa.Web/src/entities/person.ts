import { TypeEntity, TypeSimpleRecord, TypeSimpleDetail, Detail, Option, RealTypePeriod, TypePeriod, RealTypeSimple, Record } from "./entity";
import { Change } from "./process";
import { AspNetUser } from "./identity";
import { Group } from "./group";

//有證 ProfileId!=null
//訪客 No!=null
//關係人 No==null
export class Person extends TypeEntity {
    profileId: string;
    profile: Profile;
    phoneId: string;
    phone: Contact;
    emailId: string;
    email: Contact;
    addressId: string;
    address: Contact;
    emergencyId: string;
    emergency: Relationship;
    documentId: string;
    document: Document;
    experienceId: string;
    experience: Experience;
    certificateId: string;
    certificate: Certificate;
    Height: number;
    Weight: number;
    BirthDate: Date;
    sexId: string;
    sex: Option;
    changeId: string;
    change: PersonChange;
    _documentName: string;
    _documentNo: string;
    _documentTypeValue: string;
    _experienceEmployerShortName: string;
    _profileValue: string;
    experiences: Experience[];
    constructor() {
        super();
    }
}
export class PersonChange extends Change<Person>
{
    typeId: string;
    sexId: string;
    birthDate: Date;
    documentTypeId: string;
    documentNo: string;
    documentSurName: string;
    documentGivenName: string;
    documentEnglishSurName: string;
    documentEnglishGivenName: string;
    documentNationalityId: string;
    profileTypeId: string;
    profileNo: string;
    experienceEmployerId: string;
    phoneValue: string;
    relationshipOwner1DocumentSurName: string;
    relationship1OwnerDocumentGivenName: string;
    relationship1Value: string;
    relationship1OwnerPhoneValue: string;
    relationship2OwnerDocumentSurName: string;
    relationship2OwnerDocumentGivenName: string;
    relationship2Value: string;
    relationship2OwnerPhoneValue: string;
    constructor() {
        super();
    }
}

//身分證
//護照
//港澳證
//台胞證
//其他
export class Document extends TypePeriod<Person>
{
    no: string;
    surName: string;
    givenName: string;
    englishSurName: string;
    englishGivenName: string;
    birthPlaceId: string;
    birthPlace: Location;
    addressId: string;
    address: Contact;
    ethnicityId: string;
    ethnicity: Option;
    politicalStatusId: string;
    politicalStatus: Option;
    maritalStatusId: string;
    maritalStatus: Option;
    nationalityId: string;
    nationality: Option;
    authority: string;
    constructor() {
        super();
    }
}

//工作經驗
export class Experience extends TypePeriod<Person>
{
    employerId: string;
    employer: Group;
    witnessId: string;
    witness: Relationship;
    salary: string;
    leaveReason: string;
    position: string;
    constructor() {
        super();
    }
}

//健康證
//畢業證書
export class Certificate extends RealTypePeriod<Person>
{
    issuedById: string;
    issuedBy: Group;
    constructor() {
        super();
    }
}

//緊急聯絡人
//工作經驗見證人
export class Relationship extends TypeSimpleRecord<Person, Person>
{
    constructor() {
        super();
    }
}

//臨時
//駐廠
//特約
//員工
export class Profile extends RealTypeSimple {
    userId: string;
    user: AspNetUser;
    no: string;
    profileGroupId: string;
    profileGroup: ProfileGroup;
    constructor() {
        super();
    }
}


export class ProfileGroup extends Record<Profile, Group>
{
    constructor() {
        super();
    }
}


//電話
//地址
//信箱
export class Contact extends TypeSimpleDetail<Profile>
{
    name: string;
    constructor() {
        super();
    }
}

/// <summary>
/// 應聘
/// </summary>
export class Apply extends Detail<Person>
{
    needDormitoryId: string;
    needDormitory: Option;
    expectedSalary: string;
    expectedStartWorkingDate: string;
    hobbies: string;
    foreignLanguageLevel: string;
    computerSkills: string;
    constructor() {
        super();
    }
}
