import {IHoldingModel} from './HoldingModel'


export interface IWalletModel {
    id: string;
    userId: string;
    balance: number;
    profit: number;
    holdings?: IHoldingModel[];
}



