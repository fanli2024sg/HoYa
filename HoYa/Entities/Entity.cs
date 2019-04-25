using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{
    public abstract class Base
    {
        public virtual Guid Id { get; set; }
        public virtual Guid? CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        [JsonIgnore]
        public virtual Profile CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Base()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
        }
    }
    public class Definition : Base
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public virtual Guid? StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual Option Status { get; set; }
    }
    public abstract class TypeDefinition : Definition
    {
        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }
    }
    public abstract class RealTypeDefinition : TypeDefinition
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class NodeDefinition<P> : Definition
    {
        public virtual Guid? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual P Parent { get; set; }
    }
    public abstract class RealNodeDefinition<P> : NodeDefinition<P>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class TypeNodeDefinition<P> : NodeDefinition<P>
    {
        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }
    }
    public class Branch<D> : Base
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public virtual Guid? StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual Option Status { get; set; }
        public virtual Guid? DefinitionId { get; set; }
        [ForeignKey("DefinitionId")]
        [JsonIgnore]
        public virtual D Definition { get; set; }
    }
    public class Change<DB> : Base
    {
        public virtual Guid? DefinitionBranchId { get; set; }
        [ForeignKey("DefinitionBranchId")]
        public virtual DB DefinitionBranch { get; set; }
    }
    public class Detail<O> : Base
    {
        public virtual Guid? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public virtual O Owner { get; set; }
    }
    public abstract class Relation<O, T> : Detail<O>
    {
        public virtual Guid? TargetId { get; set; }
        [ForeignKey("TargetId")]
        [JsonIgnore]
        public virtual T Target { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? ArchivedDate { get; set; }

        public virtual Guid? ArchivedById { get; set; }
        [ForeignKey("ArchivedById")]
        [JsonIgnore]
        public virtual Profile ArchivedBy { get; set; }

        public Relation()
        {
        }
    }
    public abstract class SimpleDetail<O> : Detail<O>
    {
        public string Value { get; set; }
    }

    public abstract class TypeSimpleDetail<O> : SimpleDetail<O>
    {
        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }
    }
    public class Instance<DB, DC> : Base
    {
        public string No { get; set; }

        public virtual Guid? DefinitionBranchId { get; set; }
        [ForeignKey("DefinitionBranchId")]
        public virtual DB DefinitionBranch { get; set; }

        public virtual Guid? DefinitionChangeId { get; set; }
        [ForeignKey("DefinitionChangeId")]
        public virtual DC DefinitionChange { get; set; }
    }
    public abstract class SimpleInstance<DB, DC> : Instance<DB, DC>
    {
        public string Value { get; set; }
    }
    public abstract class TypeSimpleInstance<DB, DC> : SimpleInstance<DB, DC>
    {
        public virtual Guid? TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual Option Type { get; set; }
    }
    public abstract class RealSimpleInstance<DB, DC> : SimpleInstance<DB, DC>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class RealTypeSimpleInstance<DB, DC> : TypeSimpleInstance<DB, DC>
    {
        public virtual Guid? GalleryId { get; set; }
        [ForeignKey("GalleryId")]
        public virtual Folder Gallery { get; set; }
    }
    public abstract class NodeInstance<B, C, P> : Instance<B, C>
    {
        public virtual Guid? ParentId { get; set; }
        [ForeignKey("ParentId")]
        [JsonIgnore]
        public virtual P Parent { get; set; }
    } 
    public class Option : NodeDefinition<Option>
    {
    }
    public class Category : RealNodeDefinition<Category>
    {
    }
    public class Folder : Definition
    {
    }
    public class FolderFile : Relation<Folder, File>
    {
    }
    public class File : Definition
    {
        public string Path { get; set; }
        public string Url { get; set; }
    }
}
