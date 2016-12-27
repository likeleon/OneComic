using Core.Common;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
