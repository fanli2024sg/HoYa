using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{


    public abstract class Base<I>
    {
        public virtual I Id { get; set; }

        public virtual Guid? CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        [JsonIgnore]
        public virtual Profile CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public Base()
        {
            CreatedDate = DateTime.Now;

        }
    }
    public abstract class Association<O, T> : Base<Guid>
    {
        public virtual Guid? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        [JsonIgnore]
        public virtual O Owner { get; set; }

        public virtual Guid? TargetId { get; set; }
        [ForeignKey("TargetId")]
        public virtual T Target { get; set; }
    }
    public abstract class TypeAssociation<O, T> : Association<O, T>
    {
        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }
    }
    public abstract class Event<O, T> : Association<O, T>
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
    public abstract class Entity : Base<Guid>
    {
        public virtual Guid? UpdatedById { get; set; }
        [ForeignKey("UpdatedById")]
        [JsonIgnore]
        public virtual Profile UpdatedBy { get; set; }

        [JsonIgnore]
        public DateTime? UpdatedDate { get; set; }

        public Entity()
        {
            Id = Guid.NewGuid();
            UpdatedDate = DateTime.Now;
        }
    }

    public abstract class TypeEntity : Entity
    {
        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }
    }
    public abstract class RealEntity : Entity
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class RealTypeEntity : TypeEntity
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class Simple : Entity
    {
        [MaxLength(256)] public string Value { get; set; }
    }
    public abstract class TypeSimple : Simple
    {
        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }
    }

    public abstract class TypeSimpleDefinition<C> : TypeSimple
    {
        public virtual Guid? ChangeId { get; set; }

        [ForeignKey("ChangeId")]
        public virtual C Change { get; set; }
    } 
    public abstract class RealSimple : Simple
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class RealTypeSimple : TypeSimple
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class Detail<O> : Entity
    {
        public virtual Guid? OwnerId { get; set; }
        [JsonIgnore]
        [ForeignKey("OwnerId")]
        public virtual O Owner { get; set; }
    }
    public abstract class TypeDetail<O> : Detail<O>
    {
        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }
    }
    public abstract class RealDetail<O> : Detail<O>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class RealTypeDetail<O> : TypeDetail<O>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class SimpleDetail<O> : Detail<O>
    {
        [MaxLength(256)] public string Value { get; set; }
    }
    public abstract class TypeSimpleDetail<O> : SimpleDetail<O>
    {
        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }
    }
    public abstract class RealSimpleDetail<O> : SimpleDetail<O>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class RealTypeSimpleDetail<O> : TypeSimpleDetail<O>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class Period<O> : Detail<O>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public abstract class TypePeriod<O> : Period<O>
    {
        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }
    }
    public abstract class RealPeriod<O> : Period<O>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class RealTypePeriod<O> : TypePeriod<O>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class SimplePeriod<O> : Period<O>
    {
        [MaxLength(256)] public string Value { get; set; }
    }
    public abstract class TypeSimplePeriod<O> : SimplePeriod<O>
    {
        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }
    }
    public abstract class RealSimplePeriod<O> : SimplePeriod<O>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class RealTypeSimplePeriod<O> : TypeSimplePeriod<O>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }

    public abstract class Record<O, T> : Period<O>
    {
        public virtual Guid? TargetId { get; set; }
        [ForeignKey("TargetId")]
        public virtual T Target { get; set; }
    }
    public abstract class TypeRecord<O, T> : Record<O, T>
    {
        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }
    }
    public abstract class RealRecord<O, T> : Record<O, T>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class RealTypeRecord<O, T> : TypeRecord<O, T>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class SimpleRecord<O, T> : Record<O, T>
    {
        [MaxLength(256)] public string Value { get; set; }
    }
    public abstract class TypeSimpleRecord<O, T> : SimpleRecord<O, T>
    {
        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }
    }
    public abstract class RealSimpleRecord<O, T> : SimpleRecord<O, T>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class RealTypeSimpleRecord<O, T> : TypeSimpleRecord<O, T>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class Position<P> : Entity
    {
        public virtual Guid? SplitId { get; set; }
        [ForeignKey("SplitId")]
        [JsonIgnore]
        public virtual P Split { get; set; }
        public DateTime? TakeDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public virtual Guid? MergeId { get; set; }
        [ForeignKey("MergeId")]
        [JsonIgnore]
        public virtual P Merge { get; set; }
    }
    public abstract class TypePosition<P> : Position<P>
    {
        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }
    }
    public abstract class RealPosition<P> : Position<P>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class RealTypePosition<P> : TypePosition<P>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class SimplePosition<P> : Position<P>
    {
        [MaxLength(256)] public string Value { get; set; }
    }
    public abstract class TypeSimplePosition<P> : SimplePosition<P>
    {
        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }
    }
    public abstract class RealSimplePosition<P> : SimplePosition<P>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class RealTypeSimplePosition<P> : TypeSimplePosition<P>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class PositionNode<P> : Position<P>
    {
        public virtual Guid? ParentId { get; set; }
        [ForeignKey("ParentId")]
        [JsonIgnore]
        public virtual P Parent { get; set; }
    }
    public abstract class TypePositionNode<P> : PositionNode<P>
    {
        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }
    }
    public abstract class RealPositionNode<P> : PositionNode<P>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class RealTypePositionNode<P> : TypePositionNode<P>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class SimplePositionNode<P> : PositionNode<P>
    {
        [MaxLength(256)] public string Value { get; set; }
    }
    public abstract class TypeSimplePositionNode<P> : SimplePositionNode<P>
    {
        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }
    }
    public abstract class RealSimplePositionNode<P> : SimplePositionNode<P>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class RealTypeSimplePositionNode<P> : TypeSimplePositionNode<P>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class Node<P> : Entity
    {
        public virtual Guid? ParentId { get; set; }
        [ForeignKey("ParentId")]
        [JsonIgnore]
        public virtual P Parent { get; set; }
    }
    public abstract class TypeNode<P> : Node<P>
    {
        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }
    }
    public abstract class TypeNodeInstance<P,D,DC> : TypeNode<P>
    {
        public virtual Guid? DefinitionId { get; set; }
        [ForeignKey("DefinitionId")]
        public virtual D Definition { get; set; }

        public virtual Guid? DefinitionChangeId { get; set; }
        [ForeignKey("DefinitionChangeId")]
        public virtual DC DefinitionChange { get; set; }
    }
    public abstract class RealNode<P> : Node<P>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class RealTypeNode<P> : TypeNode<P>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class SimpleNode<P> : Node<P>
    {
        [MaxLength(256)] public string Value { get; set; }
    }
    public abstract class TypeSimpleNode<P> : SimpleNode<P>
    {
        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }
    }
    public abstract class RealSimpleNode<P> : SimpleNode<P>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class RealTypeSimpleNode<P> : TypeSimpleNode<P>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public class Option : SimpleNode<Option>
    {
        [MaxLength(256)] public string Code { get; set; }
    }

    public class Location : SimpleNode<Location>
    {
        [MaxLength(256)] public string Code { get; set; }
    }
    public class Folder : Simple
    {
        public virtual ICollection<FolderFile> FolderFiles { get; set; } = new HashSet<FolderFile>();
    }
    public class FolderFile : Association<Folder, File>
    {
    }
    public class File : Simple
    {
        public string Path { get; set; }
        public File()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
        }
    }

    
}
