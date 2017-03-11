using System.Windows;

namespace OneComic.Admin.Services
{
    public interface IMessageBoxService
    {
        MessageBoxResult Show(string message, string caption, MessageBoxButton button, MessageBoxImage icon);
    }
}
