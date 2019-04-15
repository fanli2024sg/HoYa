import { removeInvalidCharacters } from "@core/helpers/app.helpers";
import { Injectable } from "@angular/core";
import * as $ from "jquery";

@Injectable()
export class HeightService {
    table1Height: string;
    tbody1Height: string;
    table2Height: string;
    tbody2Height: string;
    table3Height: string;
    tbody3Height: string;
    table4Height: string;
    tbody4Height: string;
    div1Height: string;
    div2Height: string;
    div3Height: string;
    div4Height: string;
    mobileHeight: string;
    mobileDiv1Height: string;
    mobileDiv2Height: string;
    constructor() {
    }

    setMobileHeight() {
        this.mobileHeight = ($(window).height() - 40.4) + "px";
    }

    setMobileDiv1Height(add?: number) {
        this.mobileDiv1Height = ($(window).height() - 275.4 - add) + "px";
    }

    setMobileDiv2Height(add?: number) {
        this.mobileDiv2Height = ($(window).height() - 255.4 - add) + "px";
    }

    setTable1(tfoot1Height?: number) {
        if (!tfoot1Height) tfoot1Height = 0;
        this.table1Height = ($(window).height() - 289) + "px";
        this.tbody1Height = ($(window).height() - tfoot1Height - 356) + "px";
        this.removeDiv2();
    }

    setTable2() {
        this.tbody2Height = ($(window).height() - 289) + "px";
    }

    setTable4() {

        this.table4Height = ($(window).height() - 252) + "px";
        this.tbody4Height = ($(window).height() - 275) + "px";
    }

    removeDiv2() {
        this.table3Height = ($(window).height() - 252) + "px";
        this.tbody3Height = ($(window).height() - 278) + "px";
    }

    setDiv1() {//244
        this.div1Height = (Number(this.table1Height) + 100) + "px";
    }

    setDiv2(div2Height: number) {//244
        this.div2Height = div2Height + "px";
    }

    setTable3(tfoot3Height?: number, ul3Height?: number) {
        if (!tfoot3Height) tfoot3Height = 0;
        if (!ul3Height) ul3Height = 0;
        this.table3Height = (Number(this.table1Height.replace("px", "")) - Number(this.div2Height.replace("px", "")) - ul3Height - 1) + "px";
        this.tbody3Height = (Number(this.tbody1Height.replace("px", "")) - Number(this.div2Height.replace("px", "")) - tfoot3Height - 1) + "px";
        if ($(window).width() <= 768) {
            this.div2Height = "";
        }
    }
}