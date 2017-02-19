using Core.Common.Contracts;
using Core.Common.Core;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OneComic.Business.Entities
{
    [DataContract]
    public class Account : EntityBase, IIdentifiableEntity, IAccountOwnedEntity
    {
        [DataMember]
        public int AccountId { get; set; }

        [DataMember]
        public string LoginEmail { get; set; }

        public virtual ICollection<Bookmark> Bookmarks { get; set; }

        public int EntityId => AccountId;
        public int OwnerAccountId => AccountId;
    }
}
