import * as tslib_1 from "tslib";
var Base = /** @class */ (function () {
    function Base() {
        this.id = "";
        this.createdDate = new Date();
    }
    return Base;
}());
export { Base };
var Change = /** @class */ (function (_super) {
    tslib_1.__extends(Change, _super);
    function Change() {
        return _super.call(this) || this;
    }
    return Change;
}(Base));
export { Change };
var Extention = /** @class */ (function (_super) {
    tslib_1.__extends(Extention, _super);
    function Extention() {
        var _this = _super.call(this) || this;
        _this.updatedDate = new Date();
        return _this;
    }
    return Extention;
}(Base));
export { Extention };
var Simple = /** @class */ (function (_super) {
    tslib_1.__extends(Simple, _super);
    function Simple() {
        return _super.call(this) || this;
    }
    return Simple;
}(Extention));
export { Simple };
var TypeExtention = /** @class */ (function (_super) {
    tslib_1.__extends(TypeExtention, _super);
    function TypeExtention() {
        return _super.call(this) || this;
    }
    return TypeExtention;
}(Extention));
export { TypeExtention };
var Entity = /** @class */ (function (_super) {
    tslib_1.__extends(Entity, _super);
    function Entity() {
        return _super.call(this) || this;
    }
    return Entity;
}(Extention));
export { Entity };
var Definition = /** @class */ (function (_super) {
    tslib_1.__extends(Definition, _super);
    function Definition() {
        return _super.call(this) || this;
    }
    return Definition;
}(Entity));
export { Definition };
var TypeDefinition = /** @class */ (function (_super) {
    tslib_1.__extends(TypeDefinition, _super);
    function TypeDefinition() {
        return _super.call(this) || this;
    }
    return TypeDefinition;
}(Definition));
export { TypeDefinition };
var DefinitionDetail = /** @class */ (function (_super) {
    tslib_1.__extends(DefinitionDetail, _super);
    function DefinitionDetail() {
        return _super.call(this) || this;
    }
    return DefinitionDetail;
}(Definition));
export { DefinitionDetail };
var General = /** @class */ (function (_super) {
    tslib_1.__extends(General, _super);
    function General() {
        return _super.call(this) || this;
    }
    return General;
}(Base));
export { General };
var NodeGeneral = /** @class */ (function (_super) {
    tslib_1.__extends(NodeGeneral, _super);
    function NodeGeneral() {
        return _super.call(this) || this;
    }
    return NodeGeneral;
}(General));
export { NodeGeneral };
var Instance = /** @class */ (function (_super) {
    tslib_1.__extends(Instance, _super);
    function Instance() {
        return _super.call(this) || this;
    }
    return Instance;
}(Base));
export { Instance };
var Detail = /** @class */ (function (_super) {
    tslib_1.__extends(Detail, _super);
    function Detail() {
        return _super.call(this) || this;
    }
    return Detail;
}(Base));
export { Detail };
var Relation = /** @class */ (function (_super) {
    tslib_1.__extends(Relation, _super);
    function Relation() {
        var _this = _super.call(this) || this;
        _this.archivedDate = new Date();
        return _this;
    }
    return Relation;
}(Detail));
export { Relation };
var Event = /** @class */ (function (_super) {
    tslib_1.__extends(Event, _super);
    function Event() {
        return _super.call(this) || this;
    }
    return Event;
}(Relation));
export { Event };
var Node = /** @class */ (function (_super) {
    tslib_1.__extends(Node, _super);
    function Node() {
        return _super.call(this) || this;
    }
    return Node;
}(Entity));
export { Node };
var Option = /** @class */ (function (_super) {
    tslib_1.__extends(Option, _super);
    function Option() {
        return _super.call(this) || this;
    }
    return Option;
}(Node));
export { Option };
var Folder = /** @class */ (function (_super) {
    tslib_1.__extends(Folder, _super);
    function Folder() {
        return _super.call(this) || this;
    }
    return Folder;
}(Base));
export { Folder };
var FolderFile = /** @class */ (function (_super) {
    tslib_1.__extends(FolderFile, _super);
    function FolderFile() {
        return _super.call(this) || this;
    }
    return FolderFile;
}(Relation));
export { FolderFile };
var File = /** @class */ (function (_super) {
    tslib_1.__extends(File, _super);
    function File() {
        return _super.call(this) || this;
    }
    return File;
}(Base));
export { File };
//# sourceMappingURL=entity.js.map