import React, {useState}from 'react'
import {Button ,OverlayTrigger,Spinner,Tooltip} from 'react-bootstrap'
import {ICoinsMarkets} from './../../models/CoinGeckoModels'
import 'bootstrap-icons/font/bootstrap-icons.css';
import {userService} from './../../services/userService';
import {utils} from '../../utils/Utils'
import TreansactionBuyModel from '../transaction-model/TreansactionBuyModel';
import { useHistory } from 'react-router-dom';
import SpikeLineChart from '../spikeline-chart/SpikeLineChart'

interface IProps{
    coin : ICoinsMarkets;
    inWatchList : boolean;
    removeCoinFromWatchList? : (coinId: string) => void;
    addCoindToWatchlist? : (coin : ICoinsMarkets) => void;
}

const CoinTableRow: React.FC<IProps> = ({coin,inWatchList,removeCoinFromWatchList,addCoindToWatchlist}) => {

    const [modalShow, setModalShow] = useState(false);
    const [loadingStar, setLoadingStar] = useState(false)
    let history = useHistory();

    const prevPrice = utils.usePrevious(coin.current_price);
    
    const showModel = () => {
        if(!utils.isLoggedInWithNotification()) {
            return null;
        }
        setModalShow(true)
    }

    const coinOnClick =async (coin:ICoinsMarkets) =>{
        if(!utils.isLoggedInWithNotification()) {
            return null;
        }
        setLoadingStar(true);
        if(inWatchList){
            await userService.removeFromWatchList(coin.id);
            if(removeCoinFromWatchList){
                removeCoinFromWatchList(coin.id);
            }
        }
        else{
            await userService.addToWatchList(coin.id);
            if(addCoindToWatchlist){
                addCoindToWatchlist(coin);
            }
        }
        setLoadingStar(false);

    }

    function checkColor(number:number){
       return number > 0 ? "text-success" : "text-danger";
    }

    const starIcon = inWatchList? "bi bi-star-fill":"bi bi-star ";

    const overlayText = inWatchList? "Remove from watchlist!":"Add to watchlist!";

    const coinPage = (coinId:string) => history.push(`/coin/${coinId}`);

    const handleClick = async(e:any, coinId:string) => {
        if(e.target.id === 'Button') { 
            e.stopPropagation();
        }else {
            coinPage(coinId);
        }
    }

    return (
        <>
        <tr onClick={(e)=>handleClick(e,coin.id)} key={coin.id} className="align-middle clickable">
            <td >
                <OverlayTrigger
                    placement="top"
                    delay={{ show: 100, hide: 50 }}
                    overlay={<Tooltip id="button-tooltip">{overlayText}</Tooltip>}
                >
                    
                    <Button variant="link" className="p-0 shadow-none">
                        {
                        loadingStar
                        ?<Spinner variant="warning" animation="border" className="spinner-star" />
                        :<i id="Button" onClick={async () => {await coinOnClick(coin);}} className={"p-2 text-warning "+starIcon} />
                        }
                    </Button>
                </OverlayTrigger>
            </td>
            <td  className="fixed-tb-col">
                <div className="d-flex align-items-center text-start clickable">
                    <img
                        src={coin.image}
                        width="25"
                        height="25"
                        className="d-inline-block  rounded-circle mx-2 "
                        alt="logo"
                    />
                    <p className="mb-0">{coin.name}</p>
                </div>
            </td>
            <td className="d-none d-sm-table-cell">
                <p className="fw-light my-auto clickable">{coin.symbol.toUpperCase()}</p>
            </td>
            <td>
                <Button onClick={showModel} variant="outline-success" id="Button" className="mx-3 shadow-none font-sm-09">Buy</Button>
            </td>
            <td className={utils.numberChangeAnimation(prevPrice,coin.current_price)}>€{utils.getToLocalString(coin.current_price)}</td>
            
            <td className={checkColor(coin.price_change_percentage_24h)}>
                {coin.price_change_percentage_24h?.toFixed(1)}%
            </td>
            <td className={checkColor(coin.price_change_percentage_7d_in_currency)}>
                {coin.price_change_percentage_7d_in_currency?.toFixed(1)}%
            </td>
            <td>€{coin.market_cap?.toLocaleString()}</td>
            <td>€{coin.total_volume?.toLocaleString()}</td>
            <td>
                <div className="min-width-200px">
                    <SpikeLineChart data={coin.sparkline_in_7d.price}/>
                </div>
            </td>
        </tr>
        <TreansactionBuyModel
        show={modalShow}
        onHide={() => setModalShow(false)}
        coin = {coin}
    />
    </>
    )
}

export default CoinTableRow
