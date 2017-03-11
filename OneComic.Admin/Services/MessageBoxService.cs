using System.ComponentModel.Composition;
using System.Windows;

namespace OneComic.Admin.Services
{
    [Export(typeof(IMessageBoxService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class MessageBoxService : IMessageBoxService
    {
        public MessageBoxResult Show(string message, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            return MessageBox.Show(message, caption, button, icon);
        }
    }
}
