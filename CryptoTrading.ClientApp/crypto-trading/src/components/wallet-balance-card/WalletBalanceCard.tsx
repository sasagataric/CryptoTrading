import React from 'react'
import {Row} from 'react-bootstrap'
import { utils } from '../../utils/Utils'
import './WalletBalanceCard.css'


interface IProp {
    balance : number | undefined
    holding : number | undefined
}

const WalletBalanceCard : React.FC<IProp> = ({balance,holding}) => {
    let prevHolding = utils.usePrevious(holding);
    return (
        <div className="border border-radius bg-white mb-4">
            <Row>
                <h3 className="my-3 p-0 ">Wallet details</h3>
            </Row>
            <Row className=" px-2">
            <p className="fs-4">Balance : €{utils.getToLocalString(balance)}</p>
            <p className={" fs-4 " + utils.numberChangeAnimation(prevHolding,holding)}>Holding : €{utils.getToLocalString(holding)}</p>
                    
            </Row>
        </div>
    )
}

export default WalletBalanceCard
