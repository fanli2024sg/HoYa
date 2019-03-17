var CheckBox = /** @class */ (function () {
    function CheckBox() {
        this.options = new Array();
    }
    return CheckBox;
}());
export { CheckBox };
var DropDown = /** @class */ (function () {
    function DropDown(objectName, likeName) {
        this.name = objectName;
        this.collapse = false;
        this.like = "";
        this.preLike = "";
        this.pointer = 0;
        this.likeName = likeName;
        this.options = new Array();
    }
    return DropDown;
}());
export { DropDown };
var RadioButton = /** @class */ (function () {
    function RadioButton() {
        this.options = new Array();
    }
    return RadioButton;
}());
export { RadioButton };
//# sourceMappingURL=control.js.map