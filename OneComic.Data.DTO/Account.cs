using OneComic.Core;

namespace OneComic.Data.DTO
{
    public sealed class Account : ObjectBase
    {
        private int _accountId;
        private string _loginEmail;

        public int AccountId
        {
            get { return _accountId; }
            set { Set(ref _accountId, value); }
        }

        public string LoginEmail
        {
            get { return _loginEmail; }
            set { Set(ref _loginEmail, value); }
        }
    }
}
