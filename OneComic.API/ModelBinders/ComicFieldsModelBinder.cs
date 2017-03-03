namespace OneComic.API.ModelBinders
{
    public sealed class ComicFieldsModelBinder : DataFieldsModelBinder<Data.DTO.Comic>
    {
        public ComicFieldsModelBinder()
        {
            AddAssociatedType<Data.DTO.Book>(nameof(Data.Entities.Comic.Books));
        }
    }
}