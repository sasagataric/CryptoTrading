import React from 'react'
import { useHistory } from 'react-router-dom';
import {IWalletHistory} from "./../../models/WalletHistoryModel"
import { format } from 'date-fns'
import { Col, Table } from 'react-bootstrap';
import { utils } from '../../utils/Utils';
import InfoBox from '../info-box/InfoBox';

interface IProps{
    historyData : IWalletHistory[]
}
const WalletHistoryTable : React.FC<IProps> = ({historyData}) => {
    let history = useHistory();
    const coinPage = (coinId:string) => history.push(`/coin/${coinId}`);

    const textColore = (amount:number) => amount>0?"text-success":"text-danger";
    const showEmptyRows = () =>{
        return <tr className="bg-radial-gradiant-wsw text-center" >
                    <td colSpan={4} className="no-hover">
                        <Col className="d-flex justify-content-center my-4">
                            <InfoBox 
                            heading="Your portfolio is empty"
                            description="Buy any coins to get started"
                            />
                        </Col>
                    </td>
                </tr>
    }

    const showHistory = () =>{
        if(historyData.length<1){
            return showEmptyRows();
        }
        return historyData.map((data,index) =>{
            let date = new Date( data.transactionDate);;
            return <tr key={index} className="shadow rounded-pill align-middle height" >
                     <td onClick={()=>coinPage(data.coin.id)}  className=" px-1">
                         <div className="d-flex  align-items-center justify-content-center clickable text-start">
                             <img
                                 src={data.coin.image}
                                 width="25"
                                 height="25"
                                 className="d-inline-block align-top rounded-circle ms-2 me-1"
                                 alt="logo"
                             />
                             <p className=" m-0 ">{data.coin.name}</p>
                         </div>
                     </td>
                     <td  className="my-auto  px-1 text-end d-none d-sm-table-cell">
                         €{utils.getToLocalString(data.coinPriceAtTheTime)}
                    </td>
                     <td  className="my-auto px-1 text-end ">
                         <p className="m-0 mt-1 text-truncate">{utils.formatProfitNumber(data.amount*data.coinPriceAtTheTime)}</p>
                         <p className={"m-0 mb-1 " + textColore(data.amount)}>{data.amount + " " + data.coin.symbol.toUpperCase()}</p>
                    </td>
                     <td  className="my-auto  ">{format(date, 'dd/MM/yyyy HH:mm')}</td>
                 </tr>
         })
     }
    return (
        <Table responsive className=" mx-2 font-sm-09 spacing px-2 table-borderless ">
            <thead className="no-border">
                <tr >
                    <th className="p-0">Coin</th>
                    <th className="pb-0  d-none d-sm-table-cell text-end">At Price</th>
                    <th className="pb-0 px-1 text-end">Amount</th>
                    <th className="p-0">Date</th>
                </tr>
            </thead>
            <tbody >
                {showHistory()}
            </tbody>
        </Table>
    )
}

export default WalletHistoryTable
