import React from 'react'
import {IHoldingModel} from '../../models/HoldingModel'
import { ICoinsMarkets } from '../../models/CoinGeckoModels';
import { Button, Col, Row} from 'react-bootstrap'
import Percentage from "../percentage/Percentage"
import WalletHistoryTable from '../wallet-hostory-table/WalletHistoryTable'
import { utils } from '../../utils/Utils';
import { IWalletHistory } from '../../models/WalletHistoryModel';

interface IProps {
    coin : ICoinsMarkets | undefined,
    holding: IHoldingModel | undefined,
    setShowPortfolio : (value: React.SetStateAction<boolean>) => void,
    coinHistory:IWalletHistory[]
}
const HoldingCoinHistoryData : React.FC<IProps> = ({coin,holding,setShowPortfolio,coinHistory}) => {

    let prevPrice = utils.usePrevious(coin?.current_price);

    if(coin === undefined || holding === undefined) return <></>

    return <>
            <Col  className="d-flex justify-content-start mx-2 mb-3">
                <Button className="fw-bold" variant="secondary" onClick={()=>setShowPortfolio(true)}> <i className="bi bi-arrow-left-short"></i> Back</Button>
            </Col>

            <Col  className="mx-2 mb-3">
                <p className=" d-flex font-color-bluish">{coin?.name + " (" + coin?.symbol.toUpperCase() +") Balance" }</p>
                <div className="d-flex align-items-center text-start clickable">
                    <img
                        src={coin?.image}
                        width="40"
                        height="40"
                        className="d-inline-block  rounded-circle me-2 "
                        alt="logo"
                    />
                    <p className={"m-0 fs-3 fw-bold "+ utils.numberChangeAnimation(prevPrice,coin.current_price)}>€{ coin===undefined || holding?.amount=== undefined ? 0 : utils.getToLocalString(coin.current_price * holding?.amount)}</p>
                </div>
            </Col>

            <Row className="mx-0 pb-1 justify-content-start border-bottom">
                <Col xs="auto">
                    <p className="text-start mb-0 font-color-bluish">Quantity</p>
                    <p className="text-start font-color-bluish fw-bold">{holding?.amount +" "+ coin?.symbol.toUpperCase()}</p>
                </Col>
                <Col xs="auto">
                    <p className="text-start mb-0 font-color-bluish">Avg. buy price</p>
                    <p className="text-start font-color-bluish fw-bold">€{utils.getToLocalString(holding?.averageBuyingPrice)}</p>
                </Col>
                <Col xs="auto">
                    <p className="text-start mb-0 font-color-bluish">Profit/Loss</p>
                    <div className="d-flex text-start font-color-bluish fw-bold">
                        <Percentage number={(coin?.current_price/holding?.averageBuyingPrice-1)*100}/>
                        ({
                        utils.formatProfitNumber((coin?.current_price - holding?.averageBuyingPrice)* holding?.amount)
                        })
                    </div>
                </Col>
            </Row>
            
            <Col  className=" mx-0 mx-md-2 mt-4 max-height">
                    <WalletHistoryTable historyData={coinHistory} />
            </Col>

    </>
}

export default HoldingCoinHistoryData
