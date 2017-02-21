using Core.Common.Contracts;
using System.Collections.Generic;

namespace OneComic.Business.Entities
{
    public class Account : IIdentifiableEntity, IAccountOwnedEntity
    {
        public int AccountId { get; set; }

        public string LoginEmail { get; set; }

        public virtual ICollection<Bookmark> Bookmarks { get; set; }

        public int EntityId => AccountId;
        public int OwnerAccountId => AccountId;
    }
}
