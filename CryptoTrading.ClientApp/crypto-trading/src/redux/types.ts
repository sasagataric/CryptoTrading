import {IWalletModel} from "../models/WalletModel"

export type WalletState = {
    wallet: IWalletModel
}
  
export type WalletAction = {
    type: string
    payload: IWalletModel | number
}


export type DispatchType = (args: WalletAction) => WalletAction