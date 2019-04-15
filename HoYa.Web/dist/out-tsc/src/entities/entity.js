import * as tslib_1 from "tslib";
var Base = /** @class */ (function () {
    function Base() {
    }
    return Base;
}());
export { Base };
var Association = /** @class */ (function (_super) {
    tslib_1.__extends(Association, _super);
    function Association() {
        return _super.call(this) || this;
    }
    return Association;
}(Base));
export { Association };
var Event = /** @class */ (function (_super) {
    tslib_1.__extends(Event, _super);
    function Event() {
        return _super.call(this) || this;
    }
    return Event;
}(Association));
export { Event };
var Entity = /** @class */ (function (_super) {
    tslib_1.__extends(Entity, _super);
    function Entity() {
        return _super.call(this) || this;
    }
    return Entity;
}(Base));
export { Entity };
var TypeEntity = /** @class */ (function (_super) {
    tslib_1.__extends(TypeEntity, _super);
    function TypeEntity() {
        return _super.call(this) || this;
    }
    return TypeEntity;
}(Entity));
export { TypeEntity };
var RealEntity = /** @class */ (function (_super) {
    tslib_1.__extends(RealEntity, _super);
    function RealEntity() {
        return _super.call(this) || this;
    }
    return RealEntity;
}(Entity));
export { RealEntity };
var RealTypeEntity = /** @class */ (function (_super) {
    tslib_1.__extends(RealTypeEntity, _super);
    function RealTypeEntity() {
        return _super.call(this) || this;
    }
    return RealTypeEntity;
}(TypeEntity));
export { RealTypeEntity };
var Simple = /** @class */ (function (_super) {
    tslib_1.__extends(Simple, _super);
    function Simple() {
        return _super.call(this) || this;
    }
    return Simple;
}(Entity));
export { Simple };
var TypeSimple = /** @class */ (function (_super) {
    tslib_1.__extends(TypeSimple, _super);
    function TypeSimple() {
        return _super.call(this) || this;
    }
    return TypeSimple;
}(Simple));
export { TypeSimple };
var RealSimple = /** @class */ (function (_super) {
    tslib_1.__extends(RealSimple, _super);
    function RealSimple() {
        return _super.call(this) || this;
    }
    return RealSimple;
}(Simple));
export { RealSimple };
var RealTypeSimple = /** @class */ (function (_super) {
    tslib_1.__extends(RealTypeSimple, _super);
    function RealTypeSimple() {
        return _super.call(this) || this;
    }
    return RealTypeSimple;
}(TypeSimple));
export { RealTypeSimple };
var Detail = /** @class */ (function (_super) {
    tslib_1.__extends(Detail, _super);
    function Detail() {
        return _super.call(this) || this;
    }
    return Detail;
}(Entity));
export { Detail };
var TypeDetail = /** @class */ (function (_super) {
    tslib_1.__extends(TypeDetail, _super);
    function TypeDetail() {
        return _super.call(this) || this;
    }
    return TypeDetail;
}(Detail));
export { TypeDetail };
var RealDetail = /** @class */ (function (_super) {
    tslib_1.__extends(RealDetail, _super);
    function RealDetail() {
        return _super.call(this) || this;
    }
    return RealDetail;
}(Detail));
export { RealDetail };
var RealTypeDetail = /** @class */ (function (_super) {
    tslib_1.__extends(RealTypeDetail, _super);
    function RealTypeDetail() {
        return _super.call(this) || this;
    }
    return RealTypeDetail;
}(TypeDetail));
export { RealTypeDetail };
var SimpleDetail = /** @class */ (function (_super) {
    tslib_1.__extends(SimpleDetail, _super);
    function SimpleDetail() {
        return _super.call(this) || this;
    }
    return SimpleDetail;
}(Detail));
export { SimpleDetail };
var TypeSimpleDetail = /** @class */ (function (_super) {
    tslib_1.__extends(TypeSimpleDetail, _super);
    function TypeSimpleDetail() {
        return _super.call(this) || this;
    }
    return TypeSimpleDetail;
}(SimpleDetail));
export { TypeSimpleDetail };
var RealSimpleDetail = /** @class */ (function (_super) {
    tslib_1.__extends(RealSimpleDetail, _super);
    function RealSimpleDetail() {
        return _super.call(this) || this;
    }
    return RealSimpleDetail;
}(SimpleDetail));
export { RealSimpleDetail };
var RealTypeSimpleDetail = /** @class */ (function (_super) {
    tslib_1.__extends(RealTypeSimpleDetail, _super);
    function RealTypeSimpleDetail() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return RealTypeSimpleDetail;
}(TypeSimpleDetail));
export { RealTypeSimpleDetail };
var Action = /** @class */ (function (_super) {
    tslib_1.__extends(Action, _super);
    function Action() {
        return _super.call(this) || this;
    }
    return Action;
}(Detail));
export { Action };
var TypeAction = /** @class */ (function (_super) {
    tslib_1.__extends(TypeAction, _super);
    function TypeAction() {
        return _super.call(this) || this;
    }
    return TypeAction;
}(Action));
export { TypeAction };
var RealAction = /** @class */ (function (_super) {
    tslib_1.__extends(RealAction, _super);
    function RealAction() {
        return _super.call(this) || this;
    }
    return RealAction;
}(Action));
export { RealAction };
var RealTypeAction = /** @class */ (function (_super) {
    tslib_1.__extends(RealTypeAction, _super);
    function RealTypeAction() {
        return _super.call(this) || this;
    }
    return RealTypeAction;
}(TypeAction));
export { RealTypeAction };
var SimpleAction = /** @class */ (function (_super) {
    tslib_1.__extends(SimpleAction, _super);
    function SimpleAction() {
        return _super.call(this) || this;
    }
    return SimpleAction;
}(Action));
export { SimpleAction };
var TypeSimpleAction = /** @class */ (function (_super) {
    tslib_1.__extends(TypeSimpleAction, _super);
    function TypeSimpleAction() {
        return _super.call(this) || this;
    }
    return TypeSimpleAction;
}(SimpleAction));
export { TypeSimpleAction };
var RealSimpleAction = /** @class */ (function (_super) {
    tslib_1.__extends(RealSimpleAction, _super);
    function RealSimpleAction() {
        return _super.call(this) || this;
    }
    return RealSimpleAction;
}(SimpleAction));
export { RealSimpleAction };
var RealTypeSimpleAction = /** @class */ (function (_super) {
    tslib_1.__extends(RealTypeSimpleAction, _super);
    function RealTypeSimpleAction() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return RealTypeSimpleAction;
}(TypeSimpleAction));
export { RealTypeSimpleAction };
var Approval = /** @class */ (function (_super) {
    tslib_1.__extends(Approval, _super);
    function Approval() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return Approval;
}(Entity));
export { Approval };
var Period = /** @class */ (function (_super) {
    tslib_1.__extends(Period, _super);
    function Period() {
        return _super.call(this) || this;
    }
    return Period;
}(Detail));
export { Period };
var TypePeriod = /** @class */ (function (_super) {
    tslib_1.__extends(TypePeriod, _super);
    function TypePeriod() {
        return _super.call(this) || this;
    }
    return TypePeriod;
}(Period));
export { TypePeriod };
var RealPeriod = /** @class */ (function (_super) {
    tslib_1.__extends(RealPeriod, _super);
    function RealPeriod() {
        return _super.call(this) || this;
    }
    return RealPeriod;
}(Period));
export { RealPeriod };
var RealTypePeriod = /** @class */ (function (_super) {
    tslib_1.__extends(RealTypePeriod, _super);
    function RealTypePeriod() {
        return _super.call(this) || this;
    }
    return RealTypePeriod;
}(TypePeriod));
export { RealTypePeriod };
var SimplePeriod = /** @class */ (function (_super) {
    tslib_1.__extends(SimplePeriod, _super);
    function SimplePeriod() {
        return _super.call(this) || this;
    }
    return SimplePeriod;
}(Period));
export { SimplePeriod };
var TypeSimplePeriod = /** @class */ (function (_super) {
    tslib_1.__extends(TypeSimplePeriod, _super);
    function TypeSimplePeriod() {
        return _super.call(this) || this;
    }
    return TypeSimplePeriod;
}(SimplePeriod));
export { TypeSimplePeriod };
var RealSimplePeriod = /** @class */ (function (_super) {
    tslib_1.__extends(RealSimplePeriod, _super);
    function RealSimplePeriod() {
        return _super.call(this) || this;
    }
    return RealSimplePeriod;
}(SimplePeriod));
export { RealSimplePeriod };
var RealTypeSimplePeriod = /** @class */ (function (_super) {
    tslib_1.__extends(RealTypeSimplePeriod, _super);
    function RealTypeSimplePeriod() {
        return _super.call(this) || this;
    }
    return RealTypeSimplePeriod;
}(TypeSimplePeriod));
export { RealTypeSimplePeriod };
var Record = /** @class */ (function (_super) {
    tslib_1.__extends(Record, _super);
    function Record() {
        return _super.call(this) || this;
    }
    return Record;
}(Period));
export { Record };
var TypeRecord = /** @class */ (function (_super) {
    tslib_1.__extends(TypeRecord, _super);
    function TypeRecord() {
        return _super.call(this) || this;
    }
    return TypeRecord;
}(Record));
export { TypeRecord };
var RealRecord = /** @class */ (function (_super) {
    tslib_1.__extends(RealRecord, _super);
    function RealRecord() {
        return _super.call(this) || this;
    }
    return RealRecord;
}(Record));
export { RealRecord };
var RealTypeRecord = /** @class */ (function (_super) {
    tslib_1.__extends(RealTypeRecord, _super);
    function RealTypeRecord() {
        return _super.call(this) || this;
    }
    return RealTypeRecord;
}(TypeRecord));
export { RealTypeRecord };
var SimpleRecord = /** @class */ (function (_super) {
    tslib_1.__extends(SimpleRecord, _super);
    function SimpleRecord() {
        return _super.call(this) || this;
    }
    return SimpleRecord;
}(Record));
export { SimpleRecord };
var TypeSimpleRecord = /** @class */ (function (_super) {
    tslib_1.__extends(TypeSimpleRecord, _super);
    function TypeSimpleRecord() {
        return _super.call(this) || this;
    }
    return TypeSimpleRecord;
}(SimpleRecord));
export { TypeSimpleRecord };
var RealSimpleRecord = /** @class */ (function (_super) {
    tslib_1.__extends(RealSimpleRecord, _super);
    function RealSimpleRecord() {
        return _super.call(this) || this;
    }
    return RealSimpleRecord;
}(SimpleRecord));
export { RealSimpleRecord };
var RealTypeSimpleRecord = /** @class */ (function (_super) {
    tslib_1.__extends(RealTypeSimpleRecord, _super);
    function RealTypeSimpleRecord() {
        return _super.call(this) || this;
    }
    return RealTypeSimpleRecord;
}(TypeSimpleRecord));
export { RealTypeSimpleRecord };
var Position = /** @class */ (function (_super) {
    tslib_1.__extends(Position, _super);
    function Position() {
        return _super.call(this) || this;
    }
    return Position;
}(Entity));
export { Position };
var TypePosition = /** @class */ (function (_super) {
    tslib_1.__extends(TypePosition, _super);
    function TypePosition() {
        return _super.call(this) || this;
    }
    return TypePosition;
}(Position));
export { TypePosition };
var RealPosition = /** @class */ (function (_super) {
    tslib_1.__extends(RealPosition, _super);
    function RealPosition() {
        return _super.call(this) || this;
    }
    return RealPosition;
}(Position));
export { RealPosition };
var RealTypePosition = /** @class */ (function (_super) {
    tslib_1.__extends(RealTypePosition, _super);
    function RealTypePosition() {
        return _super.call(this) || this;
    }
    return RealTypePosition;
}(TypePosition));
export { RealTypePosition };
var SimplePosition = /** @class */ (function (_super) {
    tslib_1.__extends(SimplePosition, _super);
    function SimplePosition() {
        return _super.call(this) || this;
    }
    return SimplePosition;
}(Position));
export { SimplePosition };
var TypeSimplePosition = /** @class */ (function (_super) {
    tslib_1.__extends(TypeSimplePosition, _super);
    function TypeSimplePosition() {
        return _super.call(this) || this;
    }
    return TypeSimplePosition;
}(SimplePosition));
export { TypeSimplePosition };
var RealSimplePosition = /** @class */ (function (_super) {
    tslib_1.__extends(RealSimplePosition, _super);
    function RealSimplePosition() {
        return _super.call(this) || this;
    }
    return RealSimplePosition;
}(SimplePosition));
export { RealSimplePosition };
var RealTypeSimplePosition = /** @class */ (function (_super) {
    tslib_1.__extends(RealTypeSimplePosition, _super);
    function RealTypeSimplePosition() {
        return _super.call(this) || this;
    }
    return RealTypeSimplePosition;
}(TypeSimplePosition));
export { RealTypeSimplePosition };
var PositionNode = /** @class */ (function (_super) {
    tslib_1.__extends(PositionNode, _super);
    function PositionNode() {
        return _super.call(this) || this;
    }
    return PositionNode;
}(Position));
export { PositionNode };
var TypePositionNode = /** @class */ (function (_super) {
    tslib_1.__extends(TypePositionNode, _super);
    function TypePositionNode() {
        return _super.call(this) || this;
    }
    return TypePositionNode;
}(PositionNode));
export { TypePositionNode };
var RealPositionNode = /** @class */ (function (_super) {
    tslib_1.__extends(RealPositionNode, _super);
    function RealPositionNode() {
        return _super.call(this) || this;
    }
    return RealPositionNode;
}(PositionNode));
export { RealPositionNode };
var RealTypePositionNode = /** @class */ (function (_super) {
    tslib_1.__extends(RealTypePositionNode, _super);
    function RealTypePositionNode() {
        return _super.call(this) || this;
    }
    return RealTypePositionNode;
}(TypePositionNode));
export { RealTypePositionNode };
var SimplePositionNode = /** @class */ (function (_super) {
    tslib_1.__extends(SimplePositionNode, _super);
    function SimplePositionNode() {
        return _super.call(this) || this;
    }
    return SimplePositionNode;
}(PositionNode));
export { SimplePositionNode };
var TypeSimplePositionNode = /** @class */ (function (_super) {
    tslib_1.__extends(TypeSimplePositionNode, _super);
    function TypeSimplePositionNode() {
        return _super.call(this) || this;
    }
    return TypeSimplePositionNode;
}(SimplePositionNode));
export { TypeSimplePositionNode };
var RealSimplePositionNode = /** @class */ (function (_super) {
    tslib_1.__extends(RealSimplePositionNode, _super);
    function RealSimplePositionNode() {
        return _super.call(this) || this;
    }
    return RealSimplePositionNode;
}(SimplePositionNode));
export { RealSimplePositionNode };
var RealTypeSimplePositionNode = /** @class */ (function (_super) {
    tslib_1.__extends(RealTypeSimplePositionNode, _super);
    function RealTypeSimplePositionNode() {
        return _super.call(this) || this;
    }
    return RealTypeSimplePositionNode;
}(TypeSimplePositionNode));
export { RealTypeSimplePositionNode };
var Node = /** @class */ (function (_super) {
    tslib_1.__extends(Node, _super);
    function Node() {
        return _super.call(this) || this;
    }
    return Node;
}(Entity));
export { Node };
var TypeNode = /** @class */ (function (_super) {
    tslib_1.__extends(TypeNode, _super);
    function TypeNode() {
        return _super.call(this) || this;
    }
    return TypeNode;
}(Node));
export { TypeNode };
var RealNode = /** @class */ (function (_super) {
    tslib_1.__extends(RealNode, _super);
    function RealNode() {
        return _super.call(this) || this;
    }
    return RealNode;
}(Node));
export { RealNode };
var RealTypeNode = /** @class */ (function (_super) {
    tslib_1.__extends(RealTypeNode, _super);
    function RealTypeNode() {
        return _super.call(this) || this;
    }
    return RealTypeNode;
}(TypeNode));
export { RealTypeNode };
var SimpleNode = /** @class */ (function (_super) {
    tslib_1.__extends(SimpleNode, _super);
    function SimpleNode() {
        return _super.call(this) || this;
    }
    return SimpleNode;
}(Node));
export { SimpleNode };
var TypeSimpleNode = /** @class */ (function (_super) {
    tslib_1.__extends(TypeSimpleNode, _super);
    function TypeSimpleNode() {
        return _super.call(this) || this;
    }
    return TypeSimpleNode;
}(SimpleNode));
export { TypeSimpleNode };
var RealSimpleNode = /** @class */ (function (_super) {
    tslib_1.__extends(RealSimpleNode, _super);
    function RealSimpleNode() {
        return _super.call(this) || this;
    }
    return RealSimpleNode;
}(SimpleNode));
export { RealSimpleNode };
var RealTypeSimpleNode = /** @class */ (function (_super) {
    tslib_1.__extends(RealTypeSimpleNode, _super);
    function RealTypeSimpleNode() {
        return _super.call(this) || this;
    }
    return RealTypeSimpleNode;
}(TypeSimpleNode));
export { RealTypeSimpleNode };
var Option = /** @class */ (function (_super) {
    tslib_1.__extends(Option, _super);
    function Option(id, value, selected, parentId, _remark, unit, code) {
        var _this = _super.call(this) || this;
        _this.id = id;
        _this.value = value;
        _this.selected = selected;
        _this.parentId = parentId;
        _this._remark = _remark;
        _this.unit = unit;
        _this.code = code;
        return _this;
    }
    return Option;
}(SimpleNode));
export { Option };
var Folder = /** @class */ (function (_super) {
    tslib_1.__extends(Folder, _super);
    function Folder() {
        return _super.call(this) || this;
    }
    return Folder;
}(Simple));
export { Folder };
var FolderFile = /** @class */ (function (_super) {
    tslib_1.__extends(FolderFile, _super);
    function FolderFile(ownerId, targetId) {
        var _this = _super.call(this) || this;
        _this.ownerId = ownerId;
        _this.targetId = targetId;
        return _this;
    }
    return FolderFile;
}(Association));
export { FolderFile };
var File = /** @class */ (function (_super) {
    tslib_1.__extends(File, _super);
    function File() {
        return _super.call(this) || this;
    }
    return File;
}(Simple));
export { File };
//# sourceMappingURL=entity.js.map