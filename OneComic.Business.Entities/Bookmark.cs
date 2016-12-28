using Core.Common.Contracts;
using Core.Common.Core;
using System;
using System.Runtime.Serialization;

namespace OneComic.Business.Entities
{
    [DataContract]
    public sealed class Bookmark : EntityBase, IIdentifiableEntity, IAccountOwnedEntity
    {
        [DataMember]
        public int BookmarkId { get; set; }

        [DataMember]
        public int AccountId { get; set; }

        [DataMember]
        public int BookId { get; set; }

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
