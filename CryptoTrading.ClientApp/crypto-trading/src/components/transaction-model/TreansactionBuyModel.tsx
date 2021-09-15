import React, {useState} from 'react'
import {Modal ,Button, InputGroup ,FormControl} from 'react-bootstrap'
import { useDispatch, useSelector } from 'react-redux';
import { IWalletModel } from '../../models/WalletModel';
import { changeWalletBalance } from '../../redux/actions/walletActions';
import { WalletState } from '../../redux/types';
import {ICoinsMarkets} from '../../models/CoinGeckoModels'
import {holdingService} from '../../services/holdingService';
import { utils } from '../../utils/Utils'



interface IProps{
    show : boolean;
    onHide: () => void;
    coin : ICoinsMarkets ;
}
const TreansactionBuyModel : React.FC<IProps> = (props) => {

    const [amount, setAmount] = useState<number>(1);
    const [disabled, setDisabled] = useState<boolean>(false);
    const wallet : IWalletModel = useSelector((state : WalletState) => state.wallet);
    const dispatch = useDispatch();


    const handleOnChange = (event:any)=> {
        const newAmount : number = event.target.value;
        setAmount(newAmount);
    };

    const handlePriceChange = (event:any, coinPrice:number)=> {
        const newPrice : number = event.target.value;
        setAmount(newPrice/coinPrice);
    };

    const buyCoin = async (coin:ICoinsMarkets, amount:number) =>{
        setDisabled(true);
        let holding = await holdingService.buyCoin(coin.id , amount);
        if(holding){
            dispatch(changeWalletBalance(-coin.current_price * amount));
        }
        props.onHide();
        setDisabled(false);
    }

    const insufficientFundsText = (price : number) =>{
        if(wallet.balance < amount*price)
        return <p className="text-danger m-0">Insuficiant founds</p>
    }

    return (
        <Modal
        {...props}
        size="sm"
        aria-labelledby="contained-modal-title-vcenter"
        centered
        >
        <Modal.Header closeButton>
            <Modal.Title className="d-flex align-items-center fw-bold">
            Buy {props.coin.name}
            <img
                src={props.coin.image}
                width="28"
                height="28"
                className="d-inline-block align-top rounded-circle mx-2"
                alt="logo"
            />
            </Modal.Title>
        </Modal.Header>
        <Modal.Body>
            <p className="fw-light mb-2">Your balance : €{utils.getToLocalString(wallet.balance)}</p>
            <InputGroup className="mb-3">
                <InputGroup.Text>Amount</InputGroup.Text>
                <FormControl 
                className="text-end"
                onChange={handleOnChange}  
                min={0} 
                value={amount} 
                type="number"
                />
            </InputGroup>
            
            <InputGroup className="mb-1">
                <InputGroup.Text>€</InputGroup.Text>
                <FormControl 
                className="text-end"
                type="number" 
                onChange={(e)=>handlePriceChange(e,props.coin.current_price)}  
                value={amount*props.coin.current_price}
                />
            </InputGroup>
            {insufficientFundsText(props.coin.current_price)}
        </Modal.Body>
        <Modal.Footer className="d-flex justify-content-center p-1">
            <Button 
            disabled={wallet.balance<amount*props.coin.current_price || disabled ?true:false} 
            variant="success" 
            onClick={async()=>{await buyCoin(props.coin,amount);}}
            >
            Buy
            </Button>
        </Modal.Footer>
        </Modal>
    )
}

export default TreansactionBuyModel
