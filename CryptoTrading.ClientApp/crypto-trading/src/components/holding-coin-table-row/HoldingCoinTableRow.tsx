import React, {useState } from 'react'
import { ICoinsMarkets } from '../../models/CoinGeckoModels';
import TreansactionBuyModel from '../transaction-model/TreansactionBuyModel';
import TreansactionSellModel from '../transaction-model/TreansactionSellModel';
import Percentage from "../percentage/Percentage"
import {utils} from '../../utils/Utils'
import { Button} from 'react-bootstrap'
import { useHistory } from 'react-router-dom';


interface IProps {
    coin: ICoinsMarkets
    amount : number
    avgBuyPrice : number
    getHistoryForCoin :  (coinId: string) => Promise<void>
    getHoldings:  () => Promise<void>
}

const HoldingCoinTableRow : React.FC<IProps> = ({coin,amount,getHistoryForCoin,avgBuyPrice,getHoldings}) => {
    const [buyingModalShow, setBuyingModalShow] = useState(false);
    const [sellingModelShow, setSellingModelShow] = useState(false);
    let history = useHistory();
    const coinPage = (coinId:string) => history.push(`/coin/${coinId}`);

    let prevPrice = utils.usePrevious(coin.current_price);


    const handleClick = async(e:any, coinId:string) => {
        if(e.target.name === 'Button') { 
            e.stopPropagation();
        }else {
            await getHistoryForCoin(coinId)
        }
    }

    return <><tr onClick={async (e) => {await handleClick(e,coin.id)}} key={coin.id} className="align-middle clickable text-end">
                <td >
                    <div onClick={()=>coinPage(coin.id)} className="d-flex align-items-center text-start clickable my-2">
                        <img
                            src={coin.image}
                            width="25"
                            height="25"
                            className="d-inline-block  rounded-circle mx-2 "
                            alt="logo"
                        />
                        {coin.name}
                    </div>
                </td>
                <td className={utils.numberChangeAnimation(prevPrice,coin.current_price)}>€{utils.getToLocalString(coin.current_price)}</td>
                <td  >
                    <p className="m-0 mt-1 ">
                     €{utils.getToLocalString(coin.current_price * amount)}
                    </p>
                    <p className="m-0 mb-1 font-color-bluish">{amount + " " + coin.symbol.toUpperCase()}</p>
                </td>
                <td className="d-none d-md-table-cell ">€{utils.getToLocalString(avgBuyPrice)}</td>
                <td className="d-none d-sm-table-cell ">
                    <p className="m-0 mt-1 text-truncate">
                     {utils.formatProfitNumber((coin.current_price -avgBuyPrice)*amount)}
                    </p>
                    <Percentage number={(coin.current_price/avgBuyPrice-1)*100}/>
               
                </td>
                <td >
                    <div className="d-md-flex align-items-center justify-content-center text-center">
                    <Button 
                    onClick={()=>{setBuyingModalShow(true)}} 
                    name="Button" 
                    variant="outline-success" 
                    className="mx-1 shadow-none mb-1 mb-md-0 font-sm-09"
                    >Buy</Button>
                    <Button 
                    onClick={()=>{setSellingModelShow(true)}} 
                    name="Button" 
                    variant="outline-danger" 
                    className="mx-1 shadow-none mt-1 mt-md-0 font-sm-09"
                    >Sell</Button>
                    </div>
                </td>
            </tr>
            <TreansactionBuyModel
            show={buyingModalShow}
            onHide={async() => {setBuyingModalShow(false);await getHoldings();}}
            coin = {coin}
            />
            <TreansactionSellModel
            show={sellingModelShow}
            onHide={async() => { setSellingModelShow(false);await getHoldings();}}
            coin = {coin}
            maxamount = {amount}
            />
            </>
}

export default HoldingCoinTableRow
