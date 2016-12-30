using OneComic.Business.Entities;

namespace OneComic.Data.Contracts
{
    public sealed class AccountBookmarkInfo
    {
        public Account Account { get; set; }
        public Book Book { get; set; }
        public Bookmark Bookmark { get; set; }
    }
}
