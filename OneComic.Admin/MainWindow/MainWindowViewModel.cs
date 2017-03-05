using Caliburn.Micro;
using System.ComponentModel.Composition;

namespace OneComic.Admin.MainWindow
{
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class MainWindowViewModel : Conductor<IScreen>.Collection.OneActive
    {
        [ImportingConstructor]
        public MainWindowViewModel([ImportMany]IMainScreenItem[] items)
        {
            Items.AddRange(items);
        }
    }
}
