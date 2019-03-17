using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoYa.Entities
{
    public abstract class Entity
    {
        public virtual Guid Id { get; set; }

        public virtual Guid? CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        [JsonIgnore]
        public virtual Profile CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public Entity()
        {
            CreatedDate = DateTime.Now;

        }
    }
    public abstract class NodeEntity<P> : Entity
    {
        public virtual Guid? ParentId { get; set; }
        [ForeignKey("ParentId")]
        [JsonIgnore]
        public virtual P Parent { get; set; }
    }
    public abstract class Change<E> : Entity
    {
        public virtual Guid? EntityId { get; set; }
        [ForeignKey("EntityId")]
        [JsonIgnore]
        public virtual E Entity { get; set; }
    }

    public abstract class Relationship<O, OC, T, TC> : Change<O>
    {
        public virtual Guid? OwnerChangeId { get; set; }
        [ForeignKey("OwnerChangeId")]
        [JsonIgnore]
        public virtual OC OwnerChange { get; set; }

        public virtual Guid? TargetId { get; set; }
        [ForeignKey("TargetId")]
        public virtual T Target { get; set; }

        public virtual Guid? TargetChangeId { get; set; }
        [ForeignKey("TargetChangeId")]
        [JsonIgnore]
        public virtual TC TargetChange { get; set; }

        public DateTime? ArchivedDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public virtual Guid? ArchivedById { get; set; }
        [ForeignKey("ArchivedById")]
        [JsonIgnore]
        public virtual Profile ArchivedBy { get; set; }

        public Relationship()
        {
            ArchivedDate = DateTime.Now;

        }
    }
    public abstract class NodeDefinition<P, C> : Definition<C>
    {
        public virtual Guid? ParentId { get; set; }
        [ForeignKey("ParentId")]
        [JsonIgnore]
        public virtual P Parent { get; set; }
    }




    public abstract class Definition<C> : Entity
    {
        public virtual Guid? ChangeId { get; set; }
        [ForeignKey("ChangeId")]
        [JsonIgnore]
        public virtual C Change { get; set; }
    }

    public abstract class Instance<D> : Entity
    {
        public virtual Guid? DefinitionId { get; set; }
        [ForeignKey("DefinitionId")]
        [JsonIgnore]
        public virtual D Definition { get; set; }
    }

    public class Option : NodeEntity<Option>
    {
        public string Code { get; set; }
        public string Value { get; set; }
    }

    public class Folder : Definition<FolderChange>
    {

       
    }
    public class FolderChange : Change<Folder>
    {
        public string Value { get; set; }
    }
    public class FolderFile : Relationship<Folder, FolderChange, File, FileChange>
    {
    }
    public class File : Definition<FileChange>
    {

    }
    public class FileChange : Change<File>
    {
        public string Url { get; set; }
        public string Value { get; set; }
        public string Path { get; set; }
    }
}
