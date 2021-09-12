import React, { useState } from 'react'
import './ConverterToFiat.css'
import {Col, FormControl} from 'react-bootstrap'
import { utils } from '../../utils/Utils';

interface IProps{
    img:string | undefined;
    name:string | undefined;
    symbol:string | undefined;
    price:number | undefined;

}
const ConverterToFiat: React.FC<IProps> = ({img,name,symbol,price}) => {
    const [amount, setAmount] = useState<number>(1);

    const handleOnChange = (event:any)=> {
        const newAmount : number = event.target.value;
        setAmount(newAmount);
    };

    return (
        <Col>
            
            <h4 className="fw-bold font-color-bluish text-start">{symbol?.toUpperCase()} to EUR Converter</h4>
            
            <Col xs="auto" className="border border-radius p-0">
                <Col  className="d-flex px-2 py-3 ">
                    <Col xs="auto" className="p-0 ps-1">
                        <img
                        width={40}
                        height={40}
                        src={img}
                        className="d-inline-block rounded-circle "
                        alt="logo"
                        />
                    </Col>
                   
                    <Col xs="auto" className="text-start px-3">
                        <p className="m-0 font-09">{symbol?.toUpperCase()}</p>
                        <p className="m-0 fw-bold font-09">{name}</p>
                    </Col>
                    <Col  className="d-flex justify-content-end p-0">
                        <FormControl
                        onChange={handleOnChange}  
                        min={0} 
                        value={amount} 
                        type="number" 
                        className="shadow-none text-end border-0 bg-light fw-bold fs-5"
                        />
                    </Col>
                </Col>
                    
                   
                <Col  className="d-flex px-2 py-3 bg-silver border-radius-bottom">
                    <Col xs="auto" className="p-0 ps-1">
                        <img
                        width={40}
                        height={40}
                        src="https://s2.coinmarketcap.com/static/cloud/img/fiat-flags/EUR.svg"
                        className="d-inline-block rounded-circle "
                        alt="logo"
                        
                        />
                    </Col>
                   
                    <Col xs="auto" className="text-start px-3">
                        <p className="m-0 font-09">EUR</p>
                        <p className="m-0 fw-bold font-09">Euro</p>
                    </Col>
                    <Col  className="d-flex justify-content-end p-0">
                        <FormControl
                        readOnly
                        value={price !== undefined? utils.getToLocalString(amount*price) : 0 }
                        type="text" 
                        className="shadow-none text-end border-0 bg-silver fw-bold fs-5"
                        />
                    </Col>
                </Col>
            </Col>
        </Col>
    )
}

export default ConverterToFiat
