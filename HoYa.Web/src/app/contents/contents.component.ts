import { Component, OnInit } from '@angular/core';
import { MatSidenav, MatDrawerToggleResult } from '@angular/material';

@Component({
    selector: 'contents',
    templateUrl: './contents.component.html',
    styleUrls: ['./contents.component.css']
})
export class ContentsComponent implements OnInit {
    title: string;
    constructor() {
        this.title = "";
    }

    ngOnInit() { }

    toggleSideNav(sideNav: MatSidenav) {
        sideNav.toggle().then((result: any) => {
            console.log(result);
            console.log(`¿ï³æª¬ºA¡G${result.type}`);
        });
    }

    opened() {
        console.log("open");
    }

    closed() {
        console.log("close");
    }

    goto(title: string) {
        this.title = title;
    }

    back() {
        this.title = "";
    }
}
