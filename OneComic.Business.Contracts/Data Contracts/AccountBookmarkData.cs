using Core.Common.ServiceModel;
using System.Runtime.Serialization;

namespace OneComic.Business.Contracts
{
    [DataContract]
    public sealed class AccountBookmarkData : DataContractBase
    {
        [DataMember]
        public int BookmarkId { get; set; }

        [DataMember]
        public string AccountLoginEmail { get; set; }

        [DataMember]
        public string BookTitle { get; set; }

        [DataMember]
        public int PageNumber { get; set; }
    }
}
