using System.Threading.Tasks;
using System.Windows.Input;

namespace OneComic.Admin.Commands
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
