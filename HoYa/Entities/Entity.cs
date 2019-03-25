using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public DateTime? CreatedDate { get; set; }

        public Base()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
        }
    }

    public class Change : Base
    { 

    }

    public abstract class Extention: Base
    {
        public virtual Guid? UpdatedById { get; set; }
        [ForeignKey("UpdatedById")]
        [JsonIgnore]
        public virtual Profile UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Extention()
        {
            UpdatedDate = DateTime.Now;
        }
    }

    public abstract class Entity : Extention
    {
        public virtual Guid? ChangeId { get; set; }
        [ForeignKey("ChangeId")]
        [JsonIgnore]
        public virtual Change Change { get; set; }        
    }

    public abstract class Definition: Entity
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
        [JsonIgnore]
        public virtual Option Type { get; set; }
    }


    public abstract class DefinitionDetail<D, DC> : Definition
    {
        public virtual Guid? DefinitionId { get; set; }
        [ForeignKey("DefinitionId")]
        [JsonIgnore]
        public virtual D Definition { get; set; }

        public virtual Guid? DefinitionChangeId { get; set; }
        [ForeignKey("DefinitionChangeId")]
        [JsonIgnore]
        public virtual DC DefinitionChange { get; set; }
    }

    public abstract class General<D> : Base
    {
        public virtual Guid? DefinitionId { get; set; }
        [ForeignKey("DefinitionId")]
        [JsonIgnore]
        public virtual D Definition { get; set; }

        public virtual Guid? DefinitionChangeId { get; set; }
        [ForeignKey("DefinitionChangeId")]
        [JsonIgnore]
        public virtual Change DefinitionChange { get; set; }
    }

    public abstract class NodeGeneral<P, D> : General<D>
    {
        public string No { get; set; }
        public virtual Guid? ParentId { get; set; }
        [ForeignKey("ParentId")]
        [JsonIgnore]
        public virtual P Parent { get; set; } 
    }


    public abstract class Instance<DD> : Base
    {
        public string No { get; set; }
        public virtual Guid? DefinitionDetailId { get; set; }
        [ForeignKey("DefinitionDetailId")]
        [JsonIgnore]
        public virtual DD DefinitionDetail { get; set; }

        public virtual Guid? EntityChangeId { get; set; }
        [ForeignKey("EntityChangeId")]
        [JsonIgnore]
        public virtual Change EntityChange { get; set; }
    }

    public abstract class Detail<O> : Base
    {
        public virtual Guid? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        [JsonIgnore]
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
            ArchivedDate = DateTime.Now;
        }
    }

    public abstract class Event<O, OC, T, TC> : Relation<O, T>
    {
        public virtual Guid? OwnerChangeId { get; set; }
        [ForeignKey("OwnerChangeId")]
        [JsonIgnore]
        public virtual OC OwnerChange { get; set; }

        public virtual Guid? TargetChangeId { get; set; }
        [ForeignKey("TargetChangeId")]
        [JsonIgnore]
        public virtual TC TargetChange { get; set; }
    }

    public abstract class Node<P> : Entity
    {
        public virtual Guid? ParentId { get; set; }
        [ForeignKey("ParentId")]
        [JsonIgnore]
        public virtual P Parent { get; set; }
    }

    public class Option : Node<Option>
    {
        public string Value { get; set; }
        public string Code { get; set; }
    }



    public class Folder : Base
    {
        public string Value { get; set; }
    }
    public class FolderFile : Relation<Folder, File>
    {
    }
    public class File : Base
    {
        public string Path { get; set; }
        public string Url { get; set; }
        public string Value { get; set; }

    }
}
