export function removeInvalidCharacters(input) {
    if (input && input.length > 0) {
        input = input.replace("\\", "").replace("[", "").replace("]", "").replace("{", "").replace("}", "");
        if (input.indexOf("\\") != -1 || input.indexOf("{") != -1 || input.indexOf("}") != -1 || input.indexOf("[") != -1 || input.indexOf("]") != -1) {
            return this.removeInvalidCharacters(input);
        }
        else {
            return input;
        }
    }
    else {
        return input;
    }
}
//# sourceMappingURL=app.helpers.js.map