import {API} from './axios';
import jwt_decode from "jwt-decode";
import {IAccessToken} from "../models/AccessTokenModel"
import {IHoldingModel} from "../models/HoldingModel"

// @ts-ignore  
import { NotificationManager } from "react-notifications";


export const holdingService = {
    buyCoin,
    getHoldings,
    sellCoin
};

async function getHoldings()
{
    let token = localStorage.getItem('access_token');
    if(!token) {
        NotificationManager.error('Please login and try angain','Not logged in',  1500);
        return null;
    }
    const decodedToken = jwt_decode<IAccessToken>(token);   
    return await API.get<IHoldingModel[]>(`/api/holdings/${decodedToken.sub}`,)
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

async function buyCoin(coinId: string,amount :number)
{
    let token = localStorage.getItem('access_token');
    if(!token) {
        NotificationManager.error('Please login and try angain','Not logged in',  1500);
        return null;
    }
    const decodedToken = jwt_decode<IAccessToken>(token);
    const params = {
        walletId : decodedToken.wallet_id,
        coinId : coinId,
        amount : amount
    };
   
    return await API.post<IHoldingModel>(`/api/holdings/buy`,params)
                .then((res)=> {
                    NotificationManager.success(`You have successful bought ${amount} ${res.data.coin.name}`,'Successful transaction',  2500);
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

async function sellCoin(coinId: string,amount :number)
{
    let token = localStorage.getItem('access_token');
    if(!token) {
        NotificationManager.error('Please login and try angain','Not logged in',  1500);
        return null;
    }

    const decodedToken = jwt_decode<IAccessToken>(token);
    const params = {
        walletId : decodedToken.wallet_id,
        coinId : coinId,
        amount : amount
    };
   
    return await API.post<IHoldingModel>(`/api/holdings/sell`,params)
                .then((res)=> {
                    NotificationManager.success(`You have successful sold ${amount} ${res.data.coin.name}`,'Successful transaction',  2500);
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