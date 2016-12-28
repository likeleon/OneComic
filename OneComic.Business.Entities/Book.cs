using Core.Common.Contracts;
using Core.Common.Core;
using System.Runtime.Serialization;

namespace OneComic.Business.Entities
{
    [DataContract]
    public sealed class Book : EntityBase, IIdentifiableEntity
    {
        [DataMember]
        public int BookId { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Author { get; set; }

        [DataMember]
        public string Translator { get; set; }

        public int EntityId
        {
            get { return BookId; }
            set { BookId = value; }
        }
    }
}
