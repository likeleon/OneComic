using OneComic.Client.Contracts;
using OneComic.Client.Entities;
using System.ComponentModel.Composition;
using System.ServiceModel;
using System.Threading.Tasks;

namespace OneComic.Client.Proxies
{
    [Export(typeof(IAccountService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountClient : ClientBase<IAccountService>, IAccountService
    {
        public Account GetAccount(string loginEmail) => Channel.GetAccount(loginEmail);

        public Task<Account> GetAccountAsync(string loginEmail) => Channel.GetAccountAsync(loginEmail);

        public void UpdateAccount(Account account) => Channel.UpdateAccount(account);

        public Task UpdateAccountAsync(Account account) => Channel.UpdateAccountAsync(account);
    }
}
