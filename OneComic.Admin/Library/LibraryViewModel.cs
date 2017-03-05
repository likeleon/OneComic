using Caliburn.Micro;
using System.ComponentModel.Composition;

namespace OneComic.Admin.Library
{
    [Export(typeof(IMainScreenItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class LibraryViewModel : Screen, IMainScreenItem
    {
        public LibraryViewModel()
        {
            DisplayName = "라이브러리";
        }
    }
}
