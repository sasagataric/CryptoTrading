import { useEffect, useRef } from "react";
import {data} from "./coinListJSON";

// @ts-ignore 
import { NotificationManager } from "react-notifications";

const isLoggedIn = () : boolean =>{
    let token = localStorage.getItem('access_token');
    if(!token) {
        return false;
    }
    return true;
}

const isLoggedInWithNotification = () : boolean =>{
    let token = localStorage.getItem('access_token');
    if(!token) {
        NotificationManager.warning('','Please login and try angain',800);
        return false;
    }
    return true;
}

const getToLocalString = (number:number | undefined) : string =>{
    return number===undefined ? "0" : 
        number >1 
        ? number.toLocaleString(undefined, {minimumFractionDigits: 2,maximumFractionDigits: 2}) 
        : number.toLocaleString(undefined, {minimumFractionDigits: 2, maximumFractionDigits: 10});
}

const formatProfitNumber = (number : number | undefined) : string =>{
    return number===undefined ? "€0" : number===0? "€0,00" : 
    number < 0 ? 
                number < -1 
                ?"- €"+ number.toLocaleString(undefined, {minimumFractionDigits: 2, maximumFractionDigits: 2}).slice(1)
                :"- €"+ number.toLocaleString(undefined, {minimumFractionDigits: 2,maximumFractionDigits: 10}).slice(1)
    : number >1 
        ? "+ €"+ number.toLocaleString(undefined, {minimumFractionDigits: 2,maximumFractionDigits: 2}) 
        : "+ €"+ number.toLocaleString(undefined, {minimumFractionDigits: 2, maximumFractionDigits: 10})
}

const numberChangeAnimation= (prevPrice:number|undefined, newPrice:number|undefined) =>prevPrice===undefined || newPrice===undefined?" ": prevPrice === newPrice ? " ":prevPrice > newPrice ?" from-red":" from-green";

function usePrevious(value:any) {
    const ref = useRef<number>();
    useEffect(() => {
      ref.current = value;
    });
    return ref.current;
}

const findCoinIDsFromCoinDataList = (search:string) =>{
    let coin = data.filter(coin=> 
        coin.name.toLowerCase().replace( /\s/g, '')===search.toLowerCase().replace( /\s/g, '')
        || coin.symbol.toLowerCase().replace( /\s/g, '')===search.toLowerCase().replace( /\s/g, ''));
    return coin.map(c=>c.id);
}

export const utils = {
    isLoggedIn,
    isLoggedInWithNotification,
    getToLocalString,
    formatProfitNumber,
    numberChangeAnimation,
    usePrevious,
    findCoinIDsFromCoinDataList
};