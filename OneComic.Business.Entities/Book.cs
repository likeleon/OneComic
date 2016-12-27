using System.Runtime.Serialization;

namespace OneComic.Business.Entities
{
    [DataContract]
    public sealed class Book
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
    }
}
