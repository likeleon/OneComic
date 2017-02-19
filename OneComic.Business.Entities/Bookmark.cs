using Core.Common.Contracts;
using Core.Common.Core;
using System;
using System.Runtime.Serialization;

namespace OneComic.Business.Entities
{
    [DataContract]
    public class Bookmark : EntityBase, IIdentifiableEntity, IAccountOwnedEntity
    {
        [DataMember]
        public int BookmarkId { get; set; }

        [DataMember]
        public int AccountId { get; set; }

        public virtual Account Account { get; set; }

        [DataMember]
        public int BookId { get; set; }

        public virtual Book Book { get; set; }

        [DataMember]
        public int PageNumber { get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; }

        public int EntityId
        {
            get { return BookmarkId; }
            set { BookmarkId = value; }
        }

        int IAccountOwnedEntity.OwnerAccountId => AccountId;
    }
}
