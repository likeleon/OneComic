using Core.Common.Contracts;
using Core.Common.Core;
using System.Runtime.Serialization;

namespace OneComic.Business.Entities
{
    [DataContract]
    public sealed class Account : EntityBase, IIdentifiableEntity
    {
        [DataMember]
        public int AccountId { get; set; }

        [DataMember]
        public string LoginEmail { get; set; }

        public int EntityId
        {
            get { return AccountId; }
            set { AccountId = value; }
        }
    }
}
