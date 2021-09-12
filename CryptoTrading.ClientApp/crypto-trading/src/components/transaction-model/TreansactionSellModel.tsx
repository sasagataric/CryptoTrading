import React, {useState} from 'react'
import {Modal ,Button, InputGroup ,FormControl} from 'react-bootstrap'
import { useDispatch } from 'react-redux';
import { changeWalletBalance } from '../../redux/actions/walletActions';
import {ICoinsMarkets} from './../../models/CoinGeckoModels'
import {holdingService} from '../../services/holdingService';
import { utils } from '../../utils/Utils';



interface IProps{
    show : boolean;
    onHide: () => void;
    coin : ICoinsMarkets;
    maxamount : number;
}
const TreansactionSellModel : React.FC<IProps> = (props) => {

    const [amount, setAmount] = useState<number>(props.maxamount);
    const [disabled, setDisabled] = useState<boolean>(false);
    const dispatch = useDispatch();


    const handleOnChange = (event:any)=> {
        const newAmount : number = event.target.value;
        setAmount(newAmount);
    };

    const sellCoin = async (coin:ICoinsMarkets, amount:number) =>{
        setDisabled(true);
        let holding = await holdingService.sellCoin(coin.id , amount);
        if(holding){
            dispatch(changeWalletBalance(coin.current_price * amount));
        }
        props.onHide();
        setDisabled(false);

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
            Sell {props.coin.name}
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
            <InputGroup className="mb-3">
                <InputGroup.Text>Amount</InputGroup.Text>
                <FormControl 
                className="text-end"
                onChange={handleOnChange}  
                min={0}
                max={props.maxamount}
                value={amount} 
                type="number"
                />
            </InputGroup>
            
            <InputGroup className="mb-1">
                <InputGroup.Text>€</InputGroup.Text>
                <FormControl 
                className="text-end"
                type="string" 
                disabled={true} 
                value={utils.getToLocalString(amount*props.coin.current_price)}
                />
            </InputGroup>
        </Modal.Body>
        <Modal.Footer className="d-flex justify-content-center p-1">
            <Button 
            disabled={amount>props.maxamount || amount<=0 || disabled ?true:false} 
            variant="danger" 
            onClick={async()=>{await sellCoin(props.coin,amount);}}
            >
            Sell
            </Button>
        </Modal.Footer>
        </Modal>
    )
}

export default TreansactionSellModel
