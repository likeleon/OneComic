using Core.Common.Contracts;
using Core.Common.Core;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OneComic.Business.Entities
{
    [DataContract]
    public class Comic : EntityBase, IIdentifiableEntity
    {
        [DataMember]
        public int ComicId { get; set; }

        [DataMember]
        public string Title { get; set; }

        public virtual ICollection<Book> Books { get; set; }

        public int EntityId
        {
            get { return ComicId; }
            set { ComicId = value; }
        }
    }
}
