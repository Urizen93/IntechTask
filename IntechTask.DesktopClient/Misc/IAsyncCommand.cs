using System.Threading.Tasks;
using System.Windows.Input;

namespace IntechTask.DesktopClient.Misc
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync();

        bool CanExecute();
    }
}