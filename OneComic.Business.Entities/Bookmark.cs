using OneComic.Core;
using System;

namespace OneComic.Business.Entities
{
    public class Bookmark : IIdentifiableEntity, IAccountOwnedEntity
    {
        public int BookmarkId { get; set; }

        public int AccountId { get; set; }

        public virtual Account Account { get; set; }

        public int BookId { get; set; }

        public virtual Book Book { get; set; }

        public int PageNumber { get; set; }

        public DateTime DateCreated { get; set; }

        public int EntityId => BookmarkId;
        int IAccountOwnedEntity.OwnerAccountId => AccountId;
    }
}
