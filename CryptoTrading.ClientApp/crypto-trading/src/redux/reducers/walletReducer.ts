import {ActionTypes} from '../constants/action-types';
import {IWalletModel} from "../../models/WalletModel"
import {WalletAction} from '../types';

const initialState :IWalletModel = {
    balance : 0,
    id:"",
    profit:0,
    userId:"",
    holdings :[]
}
export const walletReducer = (state : IWalletModel = initialState , {type,payload} : WalletAction ) : IWalletModel =>{
    switch(type){
        case  ActionTypes.SET_WALLET: 
            return {...state, ...payload as IWalletModel};
        case  ActionTypes.CHANGE_WALLET_BALANCE: 
            return {...state, balance: state.balance + (payload as number)};
        default:
            return state;
    }
}