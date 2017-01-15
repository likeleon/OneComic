﻿namespace OneComic.Web.Core
{
    public interface ISecurityAdapter
    {
        void Initialize();

        void Register(string loginEmail, string password, object propertyValues);
        bool ChangePassword(string loginEmail, string oldPassword, string newPassword);
        bool UserExists(string loginEmail);

        bool Login(string loginEmail, string password, bool rememberMe);
        void Logout();
    }
}