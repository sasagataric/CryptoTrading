import {ICoinModel} from './CoinModel'

export interface IWalletHistory {
    id: string;
    coinId: string;
    coin: ICoinModel;
    walletId: string;
    transactionDate: Date;
    amount: number;
    coinPriceAtTheTime: number;
}