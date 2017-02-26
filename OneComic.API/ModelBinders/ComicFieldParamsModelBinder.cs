namespace OneComic.API.ModelBinders
{
    public sealed class ComicFieldParamsModelBinder : FieldParamsModelBinder<Data.DTO.Comic>
    {
        public ComicFieldParamsModelBinder()
        {
            AddAssociatedType<Data.DTO.Book>(nameof(Business.Entities.Comic.Books));
        }
    }
}