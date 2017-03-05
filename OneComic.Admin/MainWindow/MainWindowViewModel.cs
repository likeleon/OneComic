using Caliburn.Micro;
using System.ComponentModel.Composition;

namespace OneComic.Admin.MainWindow
{
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class MainWindowViewModel : Conductor<IScreen>.Collection.OneActive
    {
        public string Title => "One Comic Desktop";

        [ImportingConstructor]
        public MainWindowViewModel([ImportMany]IMainScreenTabItem[] items)
        {
            Items.AddRange(items);
        }
    }
}
