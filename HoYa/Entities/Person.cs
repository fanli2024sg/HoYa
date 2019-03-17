using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{
    //有證 ProfileId!=null
    //訪客 No!=null
    //關係人 No==null
    public class Person : TypeEntity
    {
        public virtual Guid? ProfileId { get; set; }
        [ForeignKey("ProfileId")]
        public virtual Profile Profile { get; set; }
        public virtual Guid? PhoneId { get; set; }
        [ForeignKey("PhoneId")]
        public virtual Contact Phone { get; set; }
        public virtual Guid? EmailId { get; set; }
        [ForeignKey("EmailId")]
        public virtual Contact Email { get; set; }
        public virtual Guid? AddressId { get; set; }
        [ForeignKey("AddressId")]
        public virtual Contact Address { get; set; }
        public virtual Guid? EmergencyId { get; set; }
        [ForeignKey("EmergencyId")]
        public virtual Relationship Emergency { get; set; }
        public virtual Guid? DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public virtual Document Document { get; set; }

        public virtual Guid? ExperienceId { get; set; }
        [ForeignKey("ExperienceId")]
        public virtual Experience Experience { get; set; }

        public virtual Guid? CertificateId { get; set; }
        [ForeignKey("CertificateId")]
        public virtual Certificate Certificate { get; set; }
        public float? Height { get; set; }
        public float? Weight { get; set; }
        public DateTime? BirthDate { get; set; }
        public virtual Guid? SexId { get; set; }
        [ForeignKey("SexId")]
        public virtual Option Sex { get; set; }

        public virtual Guid? ChangeId { get; set; }
        [ForeignKey("ChangeId")]
        public virtual PersonChange Change { get; set; }
    }
    public class PersonChange : Change<Person>
    {
        public Guid? TypeId { get; set; }
        public Guid? SexId { get; set; }
        public DateTime? BirthDate { get; set; }
        public Guid? DocumentTypeId { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentSurName { get; set; }
        public string DocumentGivenName { get; set; }
        public string DocumentEnglishSurName { get; set; }
        public string DocumentEnglishGivenName { get; set; }
        public Guid? DocumentNationalityId { get; set; }
        public Guid? ProfileTypeId { get; set; }
        public string ProfileNo { get; set; }
        public Guid? ExperienceEmployerId { get; set; }
        public string PhoneValue { get; set; }
        public string RelationshipOwner1DocumentSurName { get; set; }
        public string Relationship1OwnerDocumentGivenName { get; set; }
        public string Relationship1Value { get; set; }
        public string Relationship1OwnerPhoneValue { get; set; }
        public string Relationship2OwnerDocumentSurName { get; set; }
        public string Relationship2OwnerDocumentGivenName { get; set; }
        public string Relationship2Value { get; set; }
        public string Relationship2OwnerPhoneValue { get; set; }
    }

    //身分證
    //護照
    //港澳證
    //台胞證
    //其他
    public class Document : TypePeriod<Person>
    {
        [MaxLength(256)] public string No { get; set; }
        [MaxLength(256)] public string SurName { get; set; }
        [MaxLength(256)] public string GivenName { get; set; }
        [MaxLength(256)] public string EnglishSurName { get; set; }
        [MaxLength(256)] public string EnglishGivenName { get; set; }
        public virtual Guid? BirthPlaceId { get; set; }
        [ForeignKey("BirthPlaceId")]
        public virtual Location BirthPlace { get; set; }
        public virtual Guid? AddressId { get; set; }
        [ForeignKey("AddressId")]
        public virtual Contact Address { get; set; }
        public virtual Guid? EthnicityId { get; set; }
        [ForeignKey("EthnicityId")]
        public virtual Option Ethnicity { get; set; }
        public virtual Guid? PoliticalStatusId { get; set; }
        [ForeignKey("PoliticalStatusId")]
        public virtual Option PoliticalStatus { get; set; }
        public virtual Guid? MaritalStatusId { get; set; }
        [ForeignKey("MaritalStatusId")]
        public virtual Option MaritalStatus { get; set; }
        public virtual Guid? NationalityId { get; set; }
        [ForeignKey("NationalityId")]
        public virtual Option Nationality { get; set; }
        public string Authority { get; set; }
    }

    //工作經驗
    public class Experience : TypePeriod<Person>
    {
        public virtual Guid? EmployerId { get; set; }
        [ForeignKey("EmployerId")]
        public virtual Group Employer { get; set; }
        public virtual Guid? WitnessId { get; set; }
        [ForeignKey("WitnessId")]
        public virtual Relationship Witness { get; set; }
        [MaxLength(256)] public string Salary { get; set; }

        [MaxLength(256)] public string LeaveReason { get; set; }

        [MaxLength(256)] public string Position { get; set; }
    }

    //健康證
    //畢業證書
    public class Certificate : RealTypePeriod<Person>
    {
        public virtual Guid? IssuedById { get; set; }
        [ForeignKey("IssuedById")]
        public virtual Group IssuedBy { get; set; }
    }

    //緊急聯絡人
    //工作經驗見證人
    public class Relationship : TypeSimpleRecord<Person, Person>
    {
    }

    //臨時
    //駐廠
    //特約
    //員工
    public class Profile : RealTypeSimple
    {
        public virtual string UserId { get; set; }
        [ForeignKey("UserId")]
        [JsonIgnore]
        public virtual AspNetUser User { get; set; }

        public string No { get; set; }

        public virtual Guid? ProfileGroupId { get; set; }
        [ForeignKey("ProfileGroupId")]
        public virtual ProfileGroup ProfileGroup { get; set; }
    }

   

    public class ProfileGroup : Record<Profile, Group>
    {
    }


    //電話
    //地址
    //信箱
    public class Contact : TypeSimpleDetail<Profile>
    {
        [MaxLength(256)] public string Name { get; set; }
    }

    /// <summary>
    /// 應聘
    /// </summary>
    public class Apply : Detail<Person>
    {
        public virtual Guid? NeedDormitoryId { get; set; }
        [ForeignKey("NeedDormitoryId")]
        public virtual Option NeedDormitory { get; set; }
        [MaxLength(256)] public string ExpectedSalary { get; set; }
        [MaxLength(256)] public string ExpectedStartWorkingDate { get; set; }
        [MaxLength(256)] public string Hobbies { get; set; }
        [MaxLength(256)] public string ForeignLanguageLevel { get; set; }
        [MaxLength(256)] public string ComputerSkills { get; set; }
    }
}