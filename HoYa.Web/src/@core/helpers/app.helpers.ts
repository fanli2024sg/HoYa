
/*
 * Inspinia js helpers:
 *
 * correctHeight() - fix the height of main wrapper
 * detectBody() - detect windows size
 * smoothlyMenu() - add smooth fade in/out on navigation show/ide
 *
*/

import * as $ from "jquery";

export function correctHeight() {
   // $("#wrapper").height(window.innerHeight);
  //  $("#page-wrapper").css("min-height", window.innerHeight + "px");
    /*
    var pageWrapper = $("#page-wrapper");
    var navbarHeigh = $("nav.navbar-default").height();
    var wrapperHeigh = pageWrapper.height();

    if (navbarHeigh > wrapperHeigh) {
        pageWrapper.css("min-height", navbarHeigh + "px");
    }

    if (navbarHeigh < wrapperHeigh) {
       
        if (navbarHeigh < $(window).height()) {
            pageWrapper.css("min-height", $(window).height()+ "px");
            pageWrapper.css("height", $(window).height() + "px");
        } else {
            pageWrapper.css("min-height", navbarHeigh + "px");
        }
    }

    if ($("body").hasClass("fixed-nav")) {
        if (navbarHeigh > wrapperHeigh) {
            pageWrapper.css("min-height", navbarHeigh + "px");
        } else {
            pageWrapper.css("min-height", $(window).height()+ "px");
        }
    }*/
}

export function detectBody() {
    if ($(document).width() < 769) {
        $("body").addClass("body-small")
    } else {
        $("body").removeClass("body-small")
    }
}

export function smoothlyMenu() {
    if (!$("body").hasClass("mini-navbar") || $("body").hasClass("body-small")) {
        // console.log("Hide menu in order to smoothly turn on when maximize menu");
        $("#side-menu").hide();
        //  console.log("For smoothly turn on menu");
        setTimeout(
            function () {
                $("#side-menu").fadeIn(400);
            }, 200);
    } else if ($("body").hasClass("fixed-sidebar")) {
        $("#side-menu").hide();
        setTimeout(
            function () {
                $("#side-menu").fadeIn(400);
            }, 100);
    } else {
        // console.log("Remove all inline style from jquery fadeIn function to reset menu state");
        $("#side-menu").removeAttr("style");
    }
}






export function removeInvalidCharacters(input: string): string {
    if (input && input.length > 0) {
        input = input.replace("\\", "").replace("[", "").replace("]", "").replace("{", "").replace("}", "");
        if (input.indexOf("\\") != -1 || input.indexOf("{") != -1 || input.indexOf("}") != -1 || input.indexOf("[") != -1 || input.indexOf("]") != -1) {
            return this.removeInvalidCharacters(input);
        } else {
            return input;
        }
    }
    else {
        return input;
    }
}
