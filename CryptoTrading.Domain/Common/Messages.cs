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
        public const string USER_NAME_REQUIRED = "User require name.";
        public const string USER_USERNAME_REQUIRED = "User require username.";
        public const string USER_LASTNAME_REQUIRED = "User require lastname.";
        public const string USER_EMAIL_REQUIRED = "User require email in correct format.";
        public const string USER_ID_NULL = "Error occured while getting user by Id, please try again.";
        public const string USER_CREATION_ERROR = "Error occured while creating new user, please try again.";
        public const string USER_CREATION_ERROR_USERNAME_EXISTS = "Error occured while creating new user, username exists.";
        public const string USER_CREATION_ERROR_EMAIL_EXISTS = "Error occured while creating new user, email exists.";
        public const string USER_HAVE_WALLET_ERROR = "User alredy have wallet and can't have another one.";
        public const string USER_DOES_NOT_HAVE_WALLET_ERROR = "User doesn't have wallet.";
        #endregion
        #region Wallet
        public const string WALLET_CREATION_ERROR = "Error occured while creating new wallet, please try again.";
        public const string WALLET_NOT_ENOUGHT_MONEY_ERROR = "The wallet does not have enough money for the transaction.";
        public const string WALLET_ID_NULL = "Error occured while getting wallet by Id, please try again.";
        #endregion
        #region Wallet
        public const string COIN_CREATION_ERROR = "Error occured while creating new coin, please try again.";
        public const string COIN_ALREDY_EXISTS = "Coin wont be created becouse it alredy exists.";
        public const string COIN_ID_NULL = "Error occured while getting wallet by Id, please try again.";
        public const string COIN_DELETE_ERROR = "Error occured while deleting coin, please try again.";
        #endregion
    }
}
