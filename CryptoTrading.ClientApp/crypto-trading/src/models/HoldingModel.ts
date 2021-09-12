import {ICoinModel} from './CoinModel'

export interface IHoldingModel {
    coin: ICoinModel;
    walletId: string;
    amount: number;
    averageBuyingPrice: number
    averageSellingPrice: number
    DateOfFirstPurchase: Date
}