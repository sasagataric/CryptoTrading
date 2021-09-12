import React, {useState, useEffect}from 'react'
import './CoinDetails.css'
import 'bootstrap-icons/font/bootstrap-icons.css';
import {CoinGeckoService} from '../../../services/coinGeckoAPIService'
import {ICoinGeckoDataModel} from '../../../models/coin-gescko/CoinGeckoDataModel'
import {IInfoDataCards} from '../../../models/components/InfoDataCard'
import {Col, Row, Badge,Container, Spinner, Button} from 'react-bootstrap'
import PriceChart from '../../price-chart/PriceChart'
import ConverterToFiat from '../../converter-to-fiat/ConverterToFiat'
import {utils} from '../../../utils/Utils'
import CoinBadgeLinks from './components/CoinBadgeLinks'
import ProgresBar from './components/ProgresBar'
import InfoDataCard from './components/InfoDataCards'


const price24h = (price : number | undefined) =>{
    if(price !== undefined){
        if(price > 0){
            return "success";
        }
        return "danger";
    }
   return "primary";
}

const price24hChange = (number : number | undefined) =>{
   if(number !== undefined)
    return (number>0
            ?<><i className="bi bi-caret-up-fill"></i>{number.toFixed(2)}%</> 
            :<><i className="bi bi-caret-down-fill"></i>{number.toFixed(2).slice(1)}%</>
        )

    return <></>;
}

const getFDV = (price:number | undefined , maxSupply:number | undefined, totalSupply:number | undefined )=>{
    return (maxSupply!==null 
            ? price === undefined || maxSupply === undefined ? 10 : price * maxSupply
            : price === undefined || totalSupply === undefined ? 0 : price * totalSupply
    )
}  

const circulatingSupply = (maxSupply:number | undefined , price:number | undefined) => 
        price === undefined || maxSupply === undefined
        ? 0
        : (price / maxSupply) * 100;

interface IChart {
    data:{
        date : number,
        price : number
    }[]
}

const CoinDetails : React.FC = () => {
    const coinId : string = window.location.pathname.split("/")[2];
    const [coin, setCoin] = useState<ICoinGeckoDataModel>(Object);
    const [loading, setLoading] = useState(true);
    const [chartDays, setChartDays] = useState<string>("7");

    const [chartData, setChartData] = useState<IChart>({
        data:[{
            date: 0,
            price:0
        }]
    })

    const chartButtonColor = (days:string, currantDays:string) => days === currantDays ? "bg-light":"bg-silver";

    useEffect(() => {
        (async() =>{
            const data = await CoinGeckoService.getCoinData(coinId);
            if(data){
                setCoin(data);
                setLoading(false);
            }
        })()
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    useEffect(() => {
        const interval = setInterval(async () => {
            const data = await CoinGeckoService.getCoinData(coinId);
            if(data){
                setCoin(data);
                setLoading(false);
            }
        },20000)
        return () => clearInterval(interval);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

    useEffect(() => {
        (async() =>{
            const chart = await CoinGeckoService.getCoinChartData(coinId,chartDays);
            if(chart){
                const result = chart.prices.map(x => {
                    return { 
                        date: x[0],
                        price: x[1],
                     };
                });
            setChartData(prev =>({...prev, data:result}));
            }
        })()
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [chartDays])

    const infoCardData: IInfoDataCards[]= [
            {
                heading:"Market Cap",
                overlayTarget:["The market capitalization (valuation) if the max supply of a coin is in circulation. Note that it can take 3, 5, 10 or more years before the FDV can be reached, depending on how the emission schedule is designed.","Market Cap = Current Price x Circulating Supply."],
                numberWithSimbol:"€" + coin?.market_data?.market_cap?.eur.toLocaleString(),
                percentage:coin?.market_data?.market_cap_change_percentage_24h
            },
            {
                heading:"Fully Diluted Valuation",
                overlayTarget:["The total market value of a cryptocurrency's circulating supply. It is analogous to the free-float capitalization in the stock market.","FDV = Current Price x Max Supply"],
                numberWithSimbol:"€" + getFDV(coin?.market_data?.current_price?.eur, coin?.market_data?.max_supply,coin?.market_data?.circulating_supply ).toLocaleString()
            },
            {
                heading:"24 Hour Trading Vol",
                overlayTarget:["The total market value of a cryptocurrency's circulating supply. It is analogous to the free-float capitalization in the stock market.","Market Cap = Current Price x Circulating Supply."],
                numberWithSimbol:"€" + coin?.market_data?.total_volume?.eur?.toLocaleString()
            },
            {
                heading:"Circulating Supply",
                overlayTarget:["The amount of coins that are circulating in the market and are in public hands. It is analogous to the flowing shares in the stock market."],
                numberWithSimbol:utils.getToLocalString(coin?.market_data?.circulating_supply) + " " + coin?.symbol?.toUpperCase(),
                progresBar:{
                    label:circulatingSupply(coin?.market_data?.max_supply, coin?.market_data?.circulating_supply).toFixed(),
                    max:coin?.market_data?.max_supply,
                    now:coin?.market_data?.circulating_supply
                }
            }
        ]
            
  
    if (loading) {
        return <Spinner className="mt-5 spinner-color" animation="border"/>;
    }

    return (
        <Container>
            <Row className=" justify-content-md-between mb-2">
                <Col md="4" className="mx-md-3 my-1">
                    <Col className="d-flex align-items-center mb-2">
                        <img
                        src={coin?.image?.small}
                        className="d-inline-block rounded-circle "
                        alt="logo"
                        />
                        <h2 className="m-0 d-flex align-items-center ms-2 fw-bold">
                            {coin?.name}
                            <Badge bg="secondary" text="dark" pill className="font-08 bg-silver font-color-bluish mx-2">
                            {coin?.symbol?.toUpperCase()}
                            </Badge>{' '}
                        </h2>
                    </Col>
                    <Col className="d-flex align-items-center ">
                        <Badge 
                        bg="secondary" 
                        text="white" 
                        pill className="font-08 bg-silver-dark mx-1">
                            Rank #{coin?.market_cap_rank}
                        </Badge>{' '}

                        <Badge 
                        bg="secondary" 
                        text="dark" 
                        pill 
                        className="font-08 bg-silver font-color-bluish mx-1">
                           Coin
                        </Badge>{' '}
                    </Col>
                    <Row className="align-items-start my-4">
                        <CoinBadgeLinks
                        hompage={coin?.links?.homepage[0]}
                        github={coin?.links?.repos_url.github[0]}
                        reddit={coin?.links?.subreddit_url}
                        forum={coin?.links?.official_forum_url[0]}
                        />               
                    </Row> 
                </Col>
                <Col md="6" className="my-1">
                    <Col className="d-flex align-items-center justify-content-md-end mb-1">
                        <p className="font-1 m-0  font-color-bluish ">{coin?.name} Price ({coin?.symbol?.toUpperCase()})</p>
                    </Col>
                    <Col className="d-flex align-items-center  justify-content-md-end mb-2">
                        <p className="fs-1 m-0 fw-bold font-color-bluish ">
                            €{utils.getToLocalString(coin?.market_data?.current_price?.eur)}
                        </p>
                        <Badge 
                        className="p-2 font-1 ms-3" 
                        bg={price24h(coin?.market_data?.price_change_percentage_24h)}>
                            {price24hChange(coin?.market_data?.price_change_percentage_24h)}
                        </Badge>{' '}
                    </Col>
                    <Col className="d-flex align-items-center justify-content-center justify-content-md-end px-4 px-md-0">
                        <ProgresBar
                            min={coin?.market_data?.low_24h?.eur}
                            max={coin?.market_data?.high_24h?.eur}
                            currant={coin?.market_data?.current_price?.eur}
                            barSizeXS={6}
                            barSizeMD={7}
                        />
                    </Col>
                </Col>
            </Row>
            <Row className="border-top border-bottom py-2 my-2 vertical-divider justify-content-center">
                <InfoDataCard data={infoCardData}/>
            </Row>

            <Row className="justify-content-between">
                <Col lg="7" className="my- mx-md-2 p-1">
                    <Row className="mb-4 justify-content-end">
                        <Col xs="auto" className="me-4 p-1 bg-silver rounded">
                            <Button 
                            className={"p-1  shadow-none font-08 fw-bold link font-color-bluish border-0 "+ chartButtonColor("1",chartDays)}
                            onClick={()=>{setChartDays("1")}}>1D</Button>
                            <Button
                            className={"p-1  shadow-none font-08 fw-bold link font-color-bluish border-0 "+ chartButtonColor("7",chartDays)}
                            onClick={()=>{setChartDays("7")}}>7D</Button>
                            <Button 
                            className={"p-1  shadow-none font-08 fw-bold link font-color-bluish border-0 "+ chartButtonColor("30",chartDays)}
                            onClick={()=>{setChartDays("30")}}>1M</Button>
                            <Button 
                            className={"p-1  shadow-none font-08 fw-bold link font-color-bluish border-0 "+ chartButtonColor("90",chartDays)}
                            onClick={()=>{setChartDays("90")}}>3M</Button>
                            <Button 
                            className={"p-1  shadow-none font-08 fw-bold link font-color-bluish border-0 "+ chartButtonColor("365",chartDays)}
                            onClick={()=>{setChartDays("365")}}>1Y</Button>
                            <Button 
                            className={"p-1  shadow-none font-08 fw-bold link font-color-bluish border-0 "+ chartButtonColor("max",chartDays)}
                            onClick={()=>{setChartDays("max")}}>ALL</Button>
                        </Col>
                    </Row>
                    <Row className="justify-content-start">
                        <PriceChart selectedDays={chartDays} data={chartData.data}/>
                    </Row>
                </Col>
                <Col  lg={4} className="mx-2 my-4 my-lg-0 my-lg-2">
                        <ConverterToFiat 
                        img={coin?.image?.small} 
                        name={coin?.name} 
                        symbol={coin?.symbol} 
                        price={coin?.market_data?.current_price.eur}
                        />
                </Col>
            </Row>
            {
                coin?.description?.en && (
                    <Row className="mt-2 pt-3 border-top">
                    <h3 className="text-start fw-bold">What is {coin?.name}?</h3>  
                    <p className="text-start " dangerouslySetInnerHTML={{__html: coin.description.en}}></p>
                    </Row>
                )
            }
        </Container>
    )
}

export default CoinDetails
