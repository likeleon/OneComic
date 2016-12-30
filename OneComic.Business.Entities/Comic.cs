using Core.Common.Contracts;
using Core.Common.Core;
using System.Runtime.Serialization;

namespace OneComic.Business.Entities
{
    [DataContract]
    public sealed class Comic : EntityBase, IIdentifiableEntity
    {
        [DataMember]
        public int ComicId { get; set; }

        [DataMember]
        public string Title { get; set; }

        public int EntityId
        {
            get { return ComicId; }
            set { ComicId = value; }
        }
    }
}
