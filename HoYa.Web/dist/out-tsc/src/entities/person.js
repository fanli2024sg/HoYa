import * as tslib_1 from "tslib";
import { TypeEntity, TypeSimpleRecord, TypeSimpleDetail, Detail, RealTypePeriod, TypePeriod, RealTypeSimple, Record } from "./entity";
import { Change } from "./process";
//有證 ProfileId!=null
//訪客 No!=null
//關係人 No==null
var Person = /** @class */ (function (_super) {
    tslib_1.__extends(Person, _super);
    function Person() {
        return _super.call(this) || this;
    }
    return Person;
}(TypeEntity));
export { Person };
var PersonChange = /** @class */ (function (_super) {
    tslib_1.__extends(PersonChange, _super);
    function PersonChange() {
        return _super.call(this) || this;
    }
    return PersonChange;
}(Change));
export { PersonChange };
//身分證
//護照
//港澳證
//台胞證
//其他
var Document = /** @class */ (function (_super) {
    tslib_1.__extends(Document, _super);
    function Document() {
        return _super.call(this) || this;
    }
    return Document;
}(TypePeriod));
export { Document };
//工作經驗
var Experience = /** @class */ (function (_super) {
    tslib_1.__extends(Experience, _super);
    function Experience() {
        return _super.call(this) || this;
    }
    return Experience;
}(TypePeriod));
export { Experience };
//健康證
//畢業證書
var Certificate = /** @class */ (function (_super) {
    tslib_1.__extends(Certificate, _super);
    function Certificate() {
        return _super.call(this) || this;
    }
    return Certificate;
}(RealTypePeriod));
export { Certificate };
//緊急聯絡人
//工作經驗見證人
var Relationship = /** @class */ (function (_super) {
    tslib_1.__extends(Relationship, _super);
    function Relationship() {
        return _super.call(this) || this;
    }
    return Relationship;
}(TypeSimpleRecord));
export { Relationship };
//臨時
//駐廠
//特約
//員工
var Profile = /** @class */ (function (_super) {
    tslib_1.__extends(Profile, _super);
    function Profile() {
        return _super.call(this) || this;
    }
    return Profile;
}(RealTypeSimple));
export { Profile };
var ProfileGroup = /** @class */ (function (_super) {
    tslib_1.__extends(ProfileGroup, _super);
    function ProfileGroup() {
        return _super.call(this) || this;
    }
    return ProfileGroup;
}(Record));
export { ProfileGroup };
//電話
//地址
//信箱
var Contact = /** @class */ (function (_super) {
    tslib_1.__extends(Contact, _super);
    function Contact() {
        return _super.call(this) || this;
    }
    return Contact;
}(TypeSimpleDetail));
export { Contact };
/// <summary>
/// 應聘
/// </summary>
var Apply = /** @class */ (function (_super) {
    tslib_1.__extends(Apply, _super);
    function Apply() {
        return _super.call(this) || this;
    }
    return Apply;
}(Detail));
export { Apply };
//# sourceMappingURL=person.js.map