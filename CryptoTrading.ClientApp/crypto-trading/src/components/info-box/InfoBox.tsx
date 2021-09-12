import React from 'react'
import {Col} from 'react-bootstrap'
import './InfoBox.css'

interface IProps{
    heading:string,
    description?:string

}
const InfoBox : React.FC<IProps> = ({heading,description}) => {
    return (
        <Col xs={9} md={10} lg={6} className="  py-2 px-3 ">
            <h3 className="fw-bold">{heading}</h3> 
            {
                description &&
                <p className="fw-bold font-color-bluish m-0">{description}</p> 
            }
        </Col>
    )
}

export default InfoBox
