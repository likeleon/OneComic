using OneComic.Web.Core;
using System.ComponentModel.Composition;
using WebMatrix.WebData;

namespace OneComic.Web.Adapters
{
    [Export(typeof(ISecurityAdapter))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public sealed class SecurityAdapter : ISecurityAdapter
    {
        public void Initialize()
        {
            if (!WebSecurity.Initialized)
                WebSecurity.InitializeDatabaseConnection("main", "Account", "AccountId", "LoginEmail", autoCreateTables: true);
        }

        public void Register(string loginEmail, string password, object propertyValues)
        {
            WebSecurity.CreateUserAndAccount(loginEmail, password, propertyValues);
        }

        public bool Login(string loginEmail, string password, bool rememberMe)
        {
            return WebSecurity.Login(loginEmail, password, persistCookie: rememberMe);
        }

        public void Logout()
        {
            WebSecurity.Logout();
        }

        public bool ChangePassword(string loginEmail, string oldPassword, string newPassword)
        {
            return WebSecurity.ChangePassword(loginEmail, oldPassword, newPassword);
        }

        public bool UserExists(string loginEmail)
        {
            return WebSecurity.UserExists(loginEmail);
        }
    }
}