import React,{useState,useEffect} from 'react'
import CoinsTable from './../../coins-table/CoinsTable'
import {Col,Row, Spinner} from 'react-bootstrap'
import {ICoinsMarkets} from './../../../models/CoinGeckoModels'
import {userService} from './../../../services/userService'
import { CoinGeckoService } from '../../../services/coinGeckoAPIService'

const Watchlist = () => {
    const [loading,setLoading] = useState<boolean>(true);
    const [watchlist, setWatchlist] = useState<ICoinsMarkets[]> ([]);

    useEffect(() => {
        (async () => {
            const watchlistData =await userService.getWatchList();
            if(watchlistData){
                setWatchlist(watchlistData);
            }
            setLoading(false);
        })()
        
    },[])

    useEffect(() => {
        const interval = setInterval(async () => {
            if(!loading){
                const idList = watchlist?.map(c=>c.id);
                if(idList.length>0){
                    const coinsData =await CoinGeckoService.getCoinsMarkets("eur",null,null,null,idList);
                    if(coinsData){
                        setWatchlist(coinsData);
                    }
                }
            }
        },20000)
        return () => clearInterval(interval);
    }, [watchlist,loading])

    return (
        <React.Fragment>
            <div className="text-centar pb-4">
                <h1>Watchlist</h1>
            </div>
            <Row className="d-flex justify-content-center">
                {
                    loading
                    ?   <Spinner animation="border" className="spinner-color" />
                        :<Col xs="auto">
                            <CoinsTable watchlistCoins={watchlist} setWatchlist={setWatchlist}  isWatchList={true} />
                        </Col>
                }
                
            </Row>
        </React.Fragment>
    )
}

export default Watchlist
