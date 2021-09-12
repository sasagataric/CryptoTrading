import React from 'react'
import { Col, ProgressBar } from 'react-bootstrap'
import {utils} from '../../../../utils/Utils'

interface IProps{
    min:number | undefined,
    max:number | undefined,
    currant:number | undefined
    barSizeXS:number,
    barSizeMD:number
}
const ProgresBar : React.FC<IProps> = ({min,max,currant,barSizeXS,barSizeMD}) => {
    return (
        <>
           <Col xs="auto" >
                Low:<p className="fw-bold m-0"> €{utils.getToLocalString(min)}</p>
            </Col>

            <Col xs={barSizeXS} md={barSizeMD} className="mx-3">
                <ProgressBar 
                variant="secondary" 
                min={min} 
                max={max} 
                now={currant} 
                />
            </Col>
            <Col  xs="auto">
            High:<p className="fw-bold m-0"> €{utils.getToLocalString(max)}</p>
            </Col> 
        </>
    )
}

export default ProgresBar
