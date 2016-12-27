namespace OneComic.Client.Entities
{
    public sealed class Book
    {
        private int _bookId;
        private string _title;
        private string _description;
        private string _author;
        private string _translator;

        public int BookId
        {
            get { return _bookId; }
            set { _bookId = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        public string Translator
        {
            get { return _translator; }
            set { _translator = value; }
        }
    }
}
