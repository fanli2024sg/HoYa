import { Option } from "./entity";

export class CheckBox {
    options: Option[];
    hoverOption: Option;
    constructor() {
        this.options = new Array<Option>();
    }
}

export class DropDown {
    name: string;
    collapse: boolean;
    like: string;
    preLike: string;
    pointer: number;
    likeName: string;
    options: any[];
    left: string;
    width: string;
    constructor(objectName: string, likeName: string) {
        this.name = objectName;
        this.collapse = false;
        this.like = "";
        this.preLike = "";
        this.pointer = 0;
        this.likeName = likeName;
        this.options = new Array<Option>();
    }
}

export class RadioButton {
    options: Option[];
    hoverOption: Option;
    constructor() {
        this.options = new Array<Option>();
    }
}



export interface IField {
    id: string;
    value: string;
}


