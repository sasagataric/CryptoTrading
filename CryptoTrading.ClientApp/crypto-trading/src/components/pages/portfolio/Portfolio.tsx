import React, { useEffect, useState } from 'react'
import { Col, Container, Row, Table ,Spinner} from 'react-bootstrap'
import {holdingService} from '../../../services/holdingService';
import {walletService} from '../../../services/walletService';
import {IHoldingModel} from '../../../models/HoldingModel'
import { CoinGeckoService } from '../../../services/coinGeckoAPIService';
import { ICoinsMarkets } from '../../../models/CoinGeckoModels';
import { IWalletHistory } from '../../../models/WalletHistoryModel';
import HoldingCoinTableRow from '../../holding-coin-table-row/HoldingCoinTableRow'
import { utils } from '../../../utils/Utils';
import HoldingCoinHistoryData from '../../holding-coin-history-data/HoldingCoinHistoryData'
import InfoBox from '../../info-box/InfoBox';


const Portfolio = () => {
    const [holdings, setHoldings] = useState<IHoldingModel[]>([])
    const [coins,setCoins] = useState<ICoinsMarkets[]>([]);
    const [loading,setLoading] = useState<boolean>(true);
    const [showPortfolio,setShowPortfolio] = useState<boolean>(true);
    const [coinHistory, setCoinHistory] = useState<IWalletHistory[]>([]);
    const [coinId, setCoinId] = useState<string>("");

    useEffect(() => {
        (async() =>{
            const data = await holdingService.getHoldings();
            if (data) {
                setHoldings(data)
                const coinIds = data.map(data => data.coin.id);
                if(coinIds && coinIds.length>0){
                    const coinsData =await CoinGeckoService.getCoinsMarkets("eur",null,null,null,coinIds);
                    if(coinsData){
                        setCoins(coinsData);
                    }
                }
            }
            setLoading(false);
        })()
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])
    

    useEffect(() => {
        const interval = setInterval(async () => {
            if(holdings && holdings.length>0 && !loading){
                await getCoins();
            }
        },15000)
        return () => clearInterval(interval);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [holdings,loading])

    const getHoldings = async() =>{
        const data = await holdingService.getHoldings();
        if (data) {
            setHoldings(data)
        }
    } 

    const getCoins = async() =>{
        const coinIds = holdings.map(data => data.coin.id);
            if(coinIds && coinIds.length>0){
                const coinsData =await CoinGeckoService.getCoinsMarkets("eur",null,null,null,coinIds);

                if(coinsData){
                    setCoins(coinsData);
                }
            }
    } 

    const showTable = () =>{
        if(holdings){
            if(holdings.length<1){
                return <tr className="bg-radial-gradiant-sws text-center" >
                            <td colSpan={6} className="no-hover">
                                <Col className="d-flex justify-content-center my-5">
                                    <InfoBox 
                                    heading="Your portfolio is empty"
                                    description="Buy any coins to get started"
                                    />
                                </Col>
                            </td>
                        </tr>

            }
           return holdings.map(data =>{
               let coinMarketData = coins?.find(c => c.id === data.coin.id);
               if(coinMarketData) {
                return <HoldingCoinTableRow 
                        key={data.coin.id} 
                        getHoldings={getHoldings}
                        coin={coinMarketData} 
                        amount={data.amount}
                        avgBuyPrice= {data.averageBuyingPrice}
                        getHistoryForCoin={getHistoryForCoin}/>
               }
               return <></>
            })
        }
    }

    const getHistoryForCoin = async (coinId:string) =>{
        setLoading(true);
        const data = await walletService.getWalletHistoryForCoin(coinId);
        if(data){
            setCoinId(coinId);
            setCoinHistory(data);
            setShowPortfolio(false);
        }
        setLoading(false);
    }
    
    const portfolioData = () =>{
        let curranBalance=0
        if(coins){
            holdings.forEach(element => {
                let c = coins.find(c => c.id === element.coin.id)
                curranBalance += element.amount * (c===undefined?1: c.current_price);
            });
        }
        return (
            <Row className="justify-content-center">
            <Col xs="auto" className="p-0">
                <Row className="justify-content-center mb-3">
                    <Col xs="auto"  className="mx-4 fs-5">
                        <p className="font-color-bluish mb-0">Current Balance</p>
                        <p className="m-0 fs-3 fw-bold">€{ utils.getToLocalString(curranBalance)}</p>
                    </Col>
                </Row>
                <Table responsive hover className="text-end font-sm-09"  style={{width:"60vw"}}>
                    <thead>
                        <tr>
                            <th className="text-start"><p className="mb-0 ms-2">Name</p> </th>
                            <th>Price</th>
                            <th >Holdings</th>
                            <th className="d-none d-md-table-cell " >Avg. Buy Price</th>
                            <th className="d-none d-sm-table-cell " >Profit/Loss</th>
                            <th className="text-center">Buy/Sell</th>
                        </tr>
                    </thead>
                    <tbody>
                        {showTable()}
                    </tbody>
                </Table>
                {
                    holdings.length>0 &&
                    <p className="font-color-bluish my-2 font-09">Click on row for more holding details</p>
                }
            </Col>
        </Row>
        )
    }
    const coinHistoryData = () =>{
        let coin = coins.find(c => c.id === coinId);
        let holding = holdings.find(p => p.coin.id === coinId);
        if(coin === undefined || holding === undefined) return <></>
        return( 
            <Col  xs={12} sm={12} md={10} lg={9} className="mx-auto pb-4">
                <HoldingCoinHistoryData 
                coin={coin} 
                holding={holding} 
                coinHistory={coinHistory} 
                setShowPortfolio={setShowPortfolio}/>
            </Col>
        )
    }
    
    return (
        <Container >
            <h1 className="pb-3">Portfolio</h1>
            {
                loading 
                ?<Spinner animation="border" className="spinner-color" />
                : showPortfolio ? portfolioData() : coinHistoryData()
            }
        </Container>
    )
}

export default Portfolio
