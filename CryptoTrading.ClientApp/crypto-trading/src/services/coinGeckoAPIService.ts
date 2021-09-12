import {CoinGecko} from './axios';
import {ICoinsMarkets} from '../models/CoinGeckoModels';
import {ICoinGeckoDataModel} from '../models/coin-gescko/CoinGeckoDataModel';
import {ICoinChart} from '../models/coin-gescko/CoinGeckoChartModel';


// @ts-ignore  
import { NotificationManager } from "react-notifications";


export const CoinGeckoService = {
    getCoinsMarkets,
    getCoinData,
    getCoinChartData
};

async function getCoinsMarkets(currency:string,order:string | null,perPage: number | null,page:number | null,ids:string[]) 
{
    let combinedId=ids.join("%2C");

    return await CoinGecko.get<ICoinsMarkets[]> (`/coins/markets?vs_currency=${currency}&ids=${combinedId}&order=${order}&per_page=${perPage}&page=${page}&price_change_percentage=1h%2C24h%2C7d&sparkline=true`)
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

async function getCoinData(coinId:string) 
{
    return await CoinGecko.get<ICoinGeckoDataModel> (`/coins/${coinId}?localization=false&tickers=false&market_data=true&developer_data=false&sparkline=true`)
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

async function getCoinChartData(coinId:string,days:string ) 
{
    return await CoinGecko.get<ICoinChart>(`/coins/${coinId}/market_chart?vs_currency=eur&days=${days}`)
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