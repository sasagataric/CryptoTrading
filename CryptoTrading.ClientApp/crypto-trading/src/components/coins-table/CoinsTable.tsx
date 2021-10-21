import React  from 'react'
import {Col, OverlayTrigger, Spinner, Table, Tooltip} from 'react-bootstrap'
import {ICoinsMarkets} from './../../models/CoinGeckoModels'
import 'bootstrap-icons/font/bootstrap-icons.css';
import CoinTableRow from './../coin-table-row/CoinTableRow'
import InfoBox from '../info-box/InfoBox'


interface IProps{
    watchListDataloading?: boolean
    coins?: ICoinsMarkets[] | null ;
    watchlistCoins : ICoinsMarkets[];
    setWatchlist: React.Dispatch<React.SetStateAction<ICoinsMarkets[]>>;
    isWatchList : boolean;
    removeCoin? : (coinId: string) => void;
}

const CoinsTable: React.FC<IProps> = ({coins,isWatchList,watchlistCoins,setWatchlist,watchListDataloading}) => {

    const removeCoindFromWatchlist = (coinId : string) : void =>{
        let filteredCoins = watchlistCoins?.filter(coin => coin.id !== coinId);
        if(filteredCoins !== undefined){
            setWatchlist(filteredCoins);
        }
    }

    const addCoindToWatchlist = (coin : ICoinsMarkets) : void =>{
        setWatchlist( prev =>{
            if(prev){
               return [...prev ,coin]
            }
            return [coin]
        } );
    }

    const checkWatchList = (coin : ICoinsMarkets) =>{
        
        if(isWatchList === true){
            return true;
        }

        if(watchlistCoins){
            let isInWatchlist = false; 
            isInWatchlist=watchlistCoins.some(watchListCoin =>{
               return watchListCoin.id === coin.id;
           })
           return isInWatchlist;
        }

        return false;
    }

    //Show all coins(Criptocurency.tsx is parent) or show Watchlist (Watchlist.tsx is parent)
    const showCoins = ()=>{
        if(isWatchList){
            if(watchlistCoins?.length<1){
                return <tr className="bg-radial-gradiant-sws" >
                            <td colSpan={10} className="no-hover">
                                <Col className="d-flex justify-content-center my-5">
                                    <InfoBox 
                                    heading="Your watchlist is empty"
                                    description="Start building your watchlist by clicking on the star right before coin name on main page"
                                    />
                                </Col>
                            </td>
                        </tr>
            }
            return watchlistCoins?.map((coin) => (
                <CoinTableRow  
                removeCoinFromWatchList={removeCoindFromWatchlist} 
                inWatchList={checkWatchList(coin)} 
                key={coin.id} 
                coin={coin}
                />
            ));
        }

        if(coins && coins?.length<1){
            return <tr className="bg-radial-gradiant-sws" >
                        <td colSpan={10} className="no-hover">
                            <Col className="d-flex justify-content-center my-5">
                                <InfoBox 
                                heading="The searched cryptocurrency was not found"
                                description="Please try a different name"
                                />
                            </Col>
                        </td>
                    </tr>
        }
        return coins?.map((coin) => (
            <CoinTableRow
            watchListDataloading={watchListDataloading}
            addCoindToWatchlist={addCoindToWatchlist}  
            removeCoinFromWatchList={removeCoindFromWatchlist} 
            inWatchList={checkWatchList(coin)} 
            key={coin.id} 
            coin={coin}
            />
        ));

    }

    return (
            <Table  responsive hover className="fix-table font-sm-09"  style={{width:"70vw"}}>
                <thead>
                    <tr>
                        <th>
                            {
                              watchListDataloading===true &&  
                              <Spinner variant="warning" animation="border" className="spinner-star" />
                            }
                        </th>
                        <th className="text-start ps-3 fixed-tb-col" >Coin</th>
                        <th className="d-none d-sm-table-cell"></th>
                        <th >Buy</th>
                        <th >Price</th>
                        <th >24h</th>
                        <th >7d</th>
                        <th >
                            Market Cap
                            <OverlayTrigger
                                placement="auto"
                                
                                delay={{ show: 100, hide: 50 }}
                                overlay={<Tooltip id="button-tooltip">
                                    <div className="fw-bold m-1">
                                    The total market value of a cryptocurrency's circulating supply. It is analogous to the free-float capitalization in the stock market.
                                    <p className="mt-2 mb-0">Market Cap = Current Price x Circulating Supply.</p> 
                                    </div>
                                </Tooltip>}
                            >
                                <i className=" bi bi-info-circle-fill mx-2"></i>
                            </OverlayTrigger>
                        </th>
                        <th >
                            Volume(24h)
                            <OverlayTrigger
                                placement="auto"
                                
                                delay={{ show: 100, hide: 50 }}
                                overlay={<Tooltip id="button-tooltip">
                                    <div className="fw-bold m-1">
                                    A measure of how much of a cryptocurrency was traded in the last 24 hours. 
                                    </div>
                                </Tooltip>}
                            >
                                <i className=" bi bi-info-circle-fill mx-2"></i>
                            </OverlayTrigger>
                        </th>
                        <th >Last 7 Days</th>
                       
                    </tr>
                </thead>
                <tbody>
                        {showCoins()}
                </tbody>
            </Table>
    )
}

export default CoinsTable
