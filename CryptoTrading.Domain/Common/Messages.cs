using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrading.Domain.Common
{
    public static class Messages
    {
        #region User
        public const string USER_ID_REQUIRED = "User ID can't be empty.";
        public const string USER_NAME_REQUIRED = "User require name.";
        public const string USER_USERNAME_REQUIRED = "User require username.";
        public const string USER_LASTNAME_REQUIRED = "User require lastname.";
        public const string USER_EMAIL_REQUIRED = "User require email in correct format.";
        public const string USER_NOT_FOUND = "User not found.";
        public const string USERS_NOT_FOUND = "Users not found.";
        public const string USER_ID_ERROR = "Error occured while getting user by Id, please try again.";
        public const string USER_GET_BY_USERNAME = "Error occured while getting user by username, please try again.";
        public const string USER_GET_BY_EMAIL = "Error occured while getting user by email, please try again.";
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
        public const string WALLET_INSUFFICIENT_FOUNDS = "Insufficient funds in wallet";

        #endregion
        #region Coin
        public const string COIN_CREATION_ERROR = "Error occured while creating new coin, please try again.";
        public const string COIN_ALREDY_EXISTS = "Coin wont be created becouse it alredy exists.";
        public const string COIN_ID_NULL = "Error occured while getting wallet by Id, please try again.";
        public const string COIN_DELETE_ERROR = "Error occured while deleting coin, please try again.";
        #endregion
        #region CoinGecko
        public const string COINGECKO_COIN_DATA_ERROR = "Error occured while geting coin data from CoinGeckoAPI, please try again.";
        #endregion
        #region PurchasedCoin
        public const string PURCHASED_COIN_CREATION_ERROR = "Error occured while making new purchase, please try again.";
        public const string PURCHASED_CANT_BE_FOUND= "Purchase can't be found.";
        public const string PURCHASED_COIN_CANT_BE_SOLD = "The coin wasn't in a wallet so it can't be sold.";
        public const string PURCHASED_COIN_NOT_ENOUGHT_COINS_IN_WALLET = "Can't sell an Amount larger than what you have in your wallet.";
        #endregion
    }
}
