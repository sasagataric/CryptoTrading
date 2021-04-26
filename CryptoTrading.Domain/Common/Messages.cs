using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrading.Domain.Common
{
    public static class Messages
    {
        #region Users
        public const string USER_NOT_FOUND = "User does not exist.";
        public const string USER_NAME_REQUIRED = "User require name.";
        public const string USER_USERNAME_REQUIRED = "User require username.";
        public const string USER_LASTNAME_REQUIRED = "User require lastname.";
        public const string USER_EMAIL_REQUIRED = "User require email in correct format.";
        public const string USER_ID_NULL = "Error occured while getting user by Id, please try again.";
        public const string USER_CREATION_ERROR = "Error occured while creating new user, please try again.";
        public const string USER_CREATION_ERROR_USERNAME_EXISTS = "Error occured while creating new user, username exists.";
        public const string USER_CREATION_ERROR_EMAIL_EXISTS = "Error occured while creating new user, email exists.";
        #endregion
    }
}
