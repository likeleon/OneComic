using OneComic.Core;
using System;

namespace OneComic.Data.DTO
{
    public sealed class Bookmark : ObjectBase
    {
        private int _bookmarkId;
        private int _accountId;
        private int _bookId;
        private int _pageNumber;
        private DateTime _dateCreated;

        public int BookmarkId
        {
            get { return _bookmarkId; }
            set { Set(ref _bookmarkId, value); }
        }

        public int AccountId
        {
            get { return _accountId; }
            set { Set(ref _accountId, value); }
        }

        public int BookId
        {
            get { return _bookId; }
            set { Set(ref _bookId, value); }
        }

        public int PageNumber
        {
            get { return _pageNumber; }
            set { Set(ref _pageNumber, value); }
        }

        public DateTime DateCreated
        {
            get { return _dateCreated; }
            set { Set(ref _dateCreated, value); }
        }
    }
}
