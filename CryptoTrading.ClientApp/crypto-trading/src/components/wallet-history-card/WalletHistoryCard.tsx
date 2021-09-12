import React, { useRef, useState } from 'react'
import './WalletHistoryCard.css'
import {Col,Row, Button, Overlay, Tooltip, Spinner} from 'react-bootstrap'
import {IWalletHistory} from "./../../models/WalletHistoryModel"
import WalletHistoryTable from '../wallet-hostory-table/WalletHistoryTable'
import { DateRange,Range } from 'react-date-range';
import 'react-date-range/dist/styles.css'; 
import 'react-date-range/dist/theme/default.css';

interface IProps {
    walletHistory : IWalletHistory[],
    getHistoryInRange: (start: Date | undefined, end: Date | undefined) => Promise<void>,
    getHistory: () => Promise<void>,
    loadingHistoryTable: boolean
}
const WalletHistoryCard : React.FC<IProps> = ({walletHistory,getHistoryInRange,getHistory,loadingHistoryTable}) => {
    const [date, setDate] = useState<Range[]>([
        {
          startDate: new Date(),
          endDate: undefined,
          key: 'selection'
        }
    ]);

    const pickDate = async() =>{
        setShow(!show);
        await getHistoryInRange(date[0].startDate,date[0].endDate)
    }

    const [show, setShow] = useState(false);
    const target = useRef(null);
      
    return (
        <div className="border  border-radius bg-white mb-4 ">
            <Row className="justify-content-end mb-2">
                <Col md={4}>
                    <h3 className="mt-3 p-0 ">Transactions</h3>
                </Col>
                <Col md={4}>
                    <Row  className="mt-1 mt-md-3  mb-2 justify-content-center">
                        <Button 
                        variant="outline-primary" 
                        className="col-auto fw-bold   p-1 mx-1 shadow-none"
                        onClick={() => getHistory()}
                        >ALL
                        </Button>
                        <Button 
                        
                        variant="outline-primary" 
                        ref={target} 
                        onClick={() => setShow(!show)} 
                        className="col-auto py-1 fs-5  px-2 mx-1 shadow-none"
                        > <i className="bi bi-calendar-week"></i> 
                        </Button>
                    </Row>
                </Col>
            </Row>
         
            <Overlay target={target.current} show={show} placement="bottom" >
                {(props) => (
                <Tooltip bsPrefix=" p-0 m-0 bg-white " id="overlay-example" {...props}>
                   <div className="mt-1 d-flex flex-column border">
                        <DateRange
                            className=""
                            editableDateInputs={true}
                            onChange={item =>{setDate([item.selection])}}
                            moveRangeOnFirstSelection={false}
                            ranges={date}
                        />
                        <Button 
                            variant="outline-primary col-4 mx-auto mb-3" 
                            className="fw-bold  p-1 mx-1 shadow-none"
                            onClick={() => pickDate()}
                            >Serach
                        </Button>
                   </div>

                </Tooltip>
                )}
            </Overlay>

            {
                loadingHistoryTable
                ?<Spinner animation="border" className="spinner-color"/>
                :<Row  className="mx-sm-0 mx-md-2 px-2 mb-3 justify-content-center max-height">
                    <WalletHistoryTable historyData={walletHistory}/>
                </Row>
            }
            
        </div>
    )
}

export default WalletHistoryCard
