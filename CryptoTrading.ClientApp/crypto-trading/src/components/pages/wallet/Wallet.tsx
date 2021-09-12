import React , {useState, useEffect} from 'react'
import {Col,Row,Card,Button,InputGroup, FormControl, Container, Spinner} from 'react-bootstrap'
import { useSelector ,useDispatch} from 'react-redux'

import {walletService} from './../../../services/walletService'
import {IWalletModel} from "./../../../models/WalletModel"
import {IWalletHistory} from "./../../../models/WalletHistoryModel"
import WalletBalanceCard from './../../wallet-balance-card/WalletBalanceCard'
import WalletHistoryCard from './../../wallet-history-card/WalletHistoryCard'

import {WalletState} from '../../../redux/types';
import {changeWalletBalance} from '../../../redux/actions/walletActions'
import { IHoldingModel } from '../../../models/HoldingModel'
import { holdingService } from '../../../services/holdingService'
import { CoinGeckoService } from '../../../services/coinGeckoAPIService'



const Wallet = () => {
    const [loadingDetails,setLoadingDetails] = useState<boolean>(true);
    const [loadingHistory,setLoadingHistory] = useState<boolean>(true);
    const [loadingHistoryTable,setLoadingHistoryTable] = useState<boolean>(true);

    const [amount, setAmount] = useState<number>(0);
    const [walletHistory, setWalletHistory] = useState<IWalletHistory[]>([]);
    const walletRedux : IWalletModel = useSelector((state : WalletState) => state.wallet);
    const [holdings, setHoldings] = useState<IHoldingModel[]>([]);
    const [holdingAmount,setHoldingAmount] = useState<number>();


    const dispatch = useDispatch();

    useEffect(() => {
        (async()=>{
            const walletHistory = await walletService.getWalletHistory();
            if(walletHistory){
                setWalletHistory(walletHistory);
                setLoadingHistory(false);
                setLoadingHistoryTable(false);
            }
            const holdingsData = await holdingService.getHoldings();
            if (holdingsData){
                setHoldings(holdingsData);
                const coinIds = holdingsData.map(data =>data.coin.id);
                if(coinIds.length >0){
                    await calculateHoldings(holdingsData);            
                }
                setLoadingDetails(false)
            }
        })()
    }, [])

    useEffect(() => {
        const interval = setInterval(async () => {
            if(!loadingDetails){
                await calculateHoldings(holdings)
            }
        },20000)
        return () => clearInterval(interval);
    }, [holdings,loadingDetails])

    const getHistory = async() =>{
        setLoadingHistoryTable(true);
        const walletHistory = await walletService.getWalletHistory();
        if(walletHistory){
            setWalletHistory(walletHistory);
            setLoadingHistory(false);
        }
        setLoadingHistoryTable(false);

    }
    const getHistoryInRange = async(start:Date | undefined, end:Date | undefined) =>{
        if(start && end){
            setLoadingHistoryTable(true);
            const walletHistory = await walletService.getWalletHistoryInRange(start,end);
            if(walletHistory){
                setWalletHistory(walletHistory);
            }
            setLoadingHistoryTable(false);

        }

    }

    const calculateHoldings = async(holdings: IHoldingModel[]) =>{
        const coinIds = holdings.map(data =>data.coin.id);
        if(coinIds.length >0){
            const coinsData =await CoinGeckoService.getCoinsMarkets("eur",null,null,null,coinIds);
            if(coinsData){
                let holdingAmount = 0;
                holdings.forEach(element => {
                    let c = coinsData.find(c => c.id === element.coin.id)
                    holdingAmount += element.amount * (c===undefined?1: c.current_price);
                });
                setHoldingAmount(holdingAmount);
            }
        }
    }

    const handleOnChange = (event:any)=> {
        const newAmount : number = event.target.valueAsNumber;
        setAmount(newAmount);
    };

    const addBalance = async (newAmount : number) =>{
       const wallet= await walletService.addBalance(newAmount);
       if(wallet){
            dispatch(changeWalletBalance(newAmount));
        }
    }

    return (
        <Container >
            <div className="text-centar pb-4">
                <h1>Wallet</h1>
            </div>
            {
                loadingDetails && loadingHistory 
                ?<Spinner animation="border" className="spinner-color" />
                :<Row className="justify-content-evenly align-items-center">
                    <Col md={12} lg={3}>
                        <WalletBalanceCard balance={walletRedux?.balance} holding={holdingAmount}/>
                    </Col>
                    <Col md={12} lg={8}>
                        <WalletHistoryCard 
                        loadingHistoryTable={loadingHistoryTable}
                        walletHistory={walletHistory} 
                        getHistory={getHistory}
                        getHistoryInRange={getHistoryInRange}/>
                    </Col>
                </Row>
            }
            

            <h3 className="my-4">Add balance to your wallet</h3>

            <Row className="justify-content-center  gap-5 pb-5">
                <Col sm={6} md={3} className="d-flex justify-content-center my-2">
                    <Card
                        border="primary"
                        text={"dark"}
                        style={{ width: '18rem' }}
                        className="mb-2 "
                    >
                        <Card.Body>
                        <Card.Title ><span>€10.000</span>  </Card.Title>
                        <Card.Text>
                            Add to your wallet balance
                        </Card.Text>
                       
                        <Button onClick={async () => {await addBalance(10000);}}>ADD</Button>
                        
                        </Card.Body>
                    </Card>
                </Col>
                <Col sm={6} md={3} className="d-flex justify-content-center my-2">
                    <Card
                        border="primary"
                        text={"dark"}
                        style={{ width: '18rem' }}
                        className="mb-2"
                    >
                        <Card.Body>
                        <Card.Title><span>€100.000</span></Card.Title>
                        <Card.Text>
                        Add to your wallet balance
                        </Card.Text>
                        <Button onClick={async () => {await addBalance(100000);}}>ADD</Button>
                        </Card.Body>
                        
                    </Card>
                </Col>
                <Col sm={6} md={3} className="d-flex justify-content-center my-2">
                    <Card
                        border="primary"
                        text={"dark"}
                        style={{ width: '18rem' }}
                        className="mb-2"
                    >
                        <Card.Body>
                        <Card.Title><span>Custom amount</span></Card.Title>
                        <InputGroup className="mb-3">
                            <InputGroup.Text>€</InputGroup.Text>
                            <FormControl onChange={handleOnChange} min={1} type="number"/>
                        </InputGroup>
                        <Button onClick={async () => {await addBalance(amount);}}>ADD</Button>
                        </Card.Body>
                    </Card>
                </Col >
            </Row>

            

        </Container>
    )
}

export default Wallet
