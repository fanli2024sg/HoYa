import { removeInvalidCharacters } from "core/helpers/app.helpers";
import { Injectable } from "@angular/core";

@Injectable()
export class GlobalService {
    menuTypeId: string;
    menuType: {
        defaultId: string
    };

    departmentId: string;
    employeeTypeId: string;
    functionTypeId: string;
    regionId: string;
    sexTypeId: string;
    ethnicityId: string;
    politicalStatusId: string;
    groupTypeId: string;
    identificationTypeId: string;
    degreeId: string;
    maritalStatusId: string;
    educationTypeId: string;
    contactTypeId: string;
    contactType: {
        phoneId: string,
        emailId: string,
        addressId: string
    };
    phoneId: string;
    emailId: string;
    addressId: string;
    relationshipTypeId: string;
    relationshipType: {
        emergencyId: string,
        employeeId: string,
        relativeId: string,
        witnessId: string
    };
    emergencyId: string;
    witnessId: string;

    constructor() {
        this.menuTypeId = "27A264BF-7A49-4C69-BEBD-4BF8A3D81EE6";
        this.menuType = {
            defaultId: "EDD2FCE4-A972-4E0E-993E-B5ABAA0086DD"
        };

        this.departmentId = "ACB1BFB1-E352-4727-9975-582951376AA3";
        this.employeeTypeId = "B96F1D4F-05D9-4262-A207-6841858D751F";
        this.functionTypeId = "2DC759BA-8B1F-4155-B08D-AB872FC58229";
        this.regionId = "48B48487-2DD7-458C-8D72-B7DE3C9239F2";
        this.sexTypeId = "1C4461D0-9DEE-4FC2-8A83-C80D32C1E92D";
        this.ethnicityId = "2A36ACB6-195C-470E-9788-D2C64F2F22CE";
        this.politicalStatusId = "01B9D451-3C45-49DB-BD75-28A6C677FFC1";
        this.groupTypeId = "5BD930BD-3406-487C-9F63-2BBD287ED698";
        this.identificationTypeId = "444016E5-C7C4-447B-A270-3F692D0428C1";
        this.degreeId = "EE7F1DCD-4252-44C4-B215-59CA382C7FB9";
        this.maritalStatusId = "30A0C602-C323-4290-B21C-707E8B5091F7";
        this.educationTypeId = "EDDA4A83-83D3-4C95-A861-C3F447DBE2C4";

        this.contactTypeId = "E3FFAC53-71CE-4887-9742-F6C4BE5AEDFB";
        this.contactType = {
            phoneId: "1B738DCE-53E1-485A-89F0-3B79BD917790",
            emailId: "E54A69B0-44D4-4C23-9170-4341FB7F134D",
            addressId: "2A1DF43C-D157-4481-9B33-7668DB58EDC6"
        };

        this.relationshipTypeId = "5b840d0b-8d31-4005-8663-c3ad1b9087ef";
        this.relationshipType = {
            emergencyId: "07C1C722-853C-48AB-920E-4642C34625A4",
            employeeId: "D0BD4842-3B34-4193-9497-14958160F225",
            relativeId: "EFE85311-22DF-4F91-9916-88B3FA564944",
            witnessId: "134D73F9-1534-4242-A54F-AA0B9DF6A685"
        };
    }
}