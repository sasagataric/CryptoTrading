import {ActionTypes} from '../constants/action-types';
import {IWalletModel} from "../../models/WalletModel"
import {WalletAction} from '../types';

export const setWallet  = (wallet:IWalletModel) =>{
    return {
        type : ActionTypes.SET_WALLET,
        payload : wallet
    } as WalletAction
}

export const selectedWallet = (wallet:IWalletModel) =>{
    return {
        type : ActionTypes.SELECTED_WALLET,
        payload : wallet
    } as WalletAction
}

export const changeWalletBalance = (price:number) =>{
    return {
        type : ActionTypes.CHANGE_WALLET_BALANCE,
        payload : price
    } as WalletAction
}