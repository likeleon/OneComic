namespace OneComic.Data.DTO
{
    public sealed class Book
    {
        public int BookId { get; set; }
        public int ComicId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Translator { get; set; }
        public int PageCount { get; set; }
    }
}
