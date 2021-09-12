import {API} from './axios';
import jwt_decode from "jwt-decode";
import {IAccessToken} from "../models/AccessTokenModel"
import {IWalletModel} from "../models/WalletModel"
import {IWalletHistory} from '../models/WalletHistoryModel'

// @ts-ignore  
import { NotificationManager } from "react-notifications";


export const walletService = {
    getWalletById,
    addBalance,
    getWalletHistory,
    getWalletHistoryForCoin,
    getWalletHistoryInRange
};

async function getWalletById()
{
    let token = localStorage.getItem('access_token');
    if(!token) {
        // NotificationManager.error('Please login and try angain','Not logged in',  1500);
        return null;
    }

    const decodedToken = jwt_decode<IAccessToken>(token);

    return await API.get<IWalletModel>(`/api/wallets/${decodedToken.wallet_id}`)
                .then((res)=> {
                    return res.data;
                })
                .catch(error => {
                    if (error.response) {
                        NotificationManager.error(error.response.data.errorMessage ,'Error',  1500);
                        return null;
                    } else if (error.request) {
                        NotificationManager.error('An error occurred while connecting to the server' ,'Error',  1500);
                        return null;
                    } else {
                        NotificationManager.error('Something went wrong' ,'Error',  1500);
                        return null;
                    }
                });
}

async function getWalletHistory()
{
    let token = localStorage.getItem('access_token');
    if(!token) {
        NotificationManager.error('Please login and try angain','Not logged in',  1500);
        return null;
    }

    const decodedToken = jwt_decode<IAccessToken>(token);

    return await API.get<IWalletHistory[]>(`/api/wallethistory/${decodedToken.wallet_id}`)
                .then((res)=> {
                    return res.data;
                })
                .catch(error => {
                    if (error.response) {
                        NotificationManager.error(error.response.data.errorMessage ,'Error',  1500);
                        return null;
                    } else if (error.request) {
                        NotificationManager.error('An error occurred while connecting to the server' ,'Error',  1500);
                        return null;
                    } else {
                        NotificationManager.error('Something went wrong' ,'Error',  1500);
                        return null;
                    }
                });
}

async function getWalletHistoryInRange(start:Date,end:Date)
{
    let token = localStorage.getItem('access_token');
    if(!token) {
        NotificationManager.error('Please login and try angain','Not logged in',  1500);
        return null;
    }
    const decodedToken = jwt_decode<IAccessToken>(token);

    return await API.get<IWalletHistory[]>(`/api/wallethistory/${decodedToken.wallet_id}/date-range/${start.toDateString()}/${end.toDateString()}`)
                .then((res)=> {
                    return res.data;
                })
                .catch(error => {
                    if (error.response) {
                        NotificationManager.error(error.response.data.errorMessage ,'Error',  1500);
                        return null;
                    } else if (error.request) {
                        NotificationManager.error('An error occurred while connecting to the server' ,'Error',  1500);
                        return null;
                    } else {
                        NotificationManager.error('Something went wrong' ,'Error',  1500);
                        return null;
                    }
                });
}

async function getWalletHistoryForCoin(coinId: string)
{
    let token = localStorage.getItem('access_token');
    if(!token) {
        NotificationManager.error('Please login and try angain','Not logged in',  1500);
        return null;
    }

    const decodedToken = jwt_decode<IAccessToken>(token);

    return await API.get<IWalletHistory[]>(`/api/wallethistory/${decodedToken.wallet_id}/coin/${coinId}`)
                .then((res)=> {
                    return res.data;
                })
                .catch(error => {
                    if (error.response) {
                        NotificationManager.error(error.response.data.errorMessage ,'Error',  1500);
                        return null;
                    } else if (error.request) {
                        NotificationManager.error('An error occurred while connecting to the server' ,'Error',  1500);
                        return null;
                    } else {
                        NotificationManager.error('Something went wrong' ,'Error',  1500);
                        return null;
                    }
                });
}

async function addBalance(amount: number)
{
    let token = localStorage.getItem('access_token');
    if(!token) {
        NotificationManager.error('Please login and try angain','Not logged in',  1500);
        return null;
    }

    const decodedToken = jwt_decode<IAccessToken>(token);
    const params = {
        walletId : decodedToken.wallet_id,
        amount : amount
    };
    const config = {
        headers: {
          'Content-Type': 'application/json'
        }
      }
    return await API.post<IWalletModel>("/api/wallets/add-balance", params, config)
                .then((res)=> {
                    NotificationManager.success(`You have successfully added €${amount} to your wallet balance`);
                    return res.data;
                })
                .catch(error => {
                    if (error.response) {
                        NotificationManager.error(error.response.data.errorMessage ,'Error',  1500);
                        return null;
                    } else if (error.request) {
                        NotificationManager.error('An error occurred while connecting to the server' ,'Error',  1500);
                        return null;
                    } else {
                        NotificationManager.error('Something went wrong' ,'Error',  1500);
                        return null;
                    }
                });
}
