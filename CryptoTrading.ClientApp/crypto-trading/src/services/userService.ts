import {API} from './axios';
import jwt_decode from "jwt-decode";
import {IAccessToken} from "../models/AccessTokenModel"
import {ICoinsMarkets} from "../models/CoinGeckoModels"

// @ts-ignore  
import { NotificationManager } from "react-notifications";


export const userService = {
    addToWatchList,
    getUsers,
    getWatchList,
    removeFromWatchList
};

async function getUsers()
{
    return await API.get("/api/Users")
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

async function addToWatchList(coinId: string)
{
    let token = localStorage.getItem('access_token');
    if(!token) {
        NotificationManager.error('Please login and try angain','Not logged in',  1500);
        return null;
    }

    const decodedToken = jwt_decode<IAccessToken>(token);

    const params = {
        userId : decodedToken.sub,
        coinId : coinId
    };
    const config = {
        headers: {
          'Content-Type': 'application/json'
        }
      }
    return await API.post("/api/Users/watch-list/add", params, config)
                .then((res)=> {
                    NotificationManager.success('A coin was successfully added to your watchlist','Added',  1000);
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

async function removeFromWatchList(coinId: string)
{
    let token = localStorage.getItem('access_token');
    if(!token) {
        NotificationManager.error('Please login and try angain','Not logged in',  1500);
        return null;
    }

    const decodedToken = jwt_decode<IAccessToken>(token);

    const params = {
        userId : decodedToken.sub,
        coinId : coinId
    };
    const config = {
        headers: {
          'Content-Type': 'application/json'
        }
      }
    return await API.post("/api/Users/watch-list/remove", params, config)
                .then((res)=> {
                    NotificationManager.success('A coin was successfully removed from your watchlist','Removed',  1000);
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

async function getWatchList()
{
    let token = localStorage.getItem('access_token');
    if(!token) {
        // NotificationManager.error('Please login and try angain','Not logged in',  1500);
        return null;
    }

    const decodedToken = jwt_decode<IAccessToken>(token);

    return await API.get<ICoinsMarkets[]>(`/api/Users/${decodedToken.sub}/watch-list`)
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