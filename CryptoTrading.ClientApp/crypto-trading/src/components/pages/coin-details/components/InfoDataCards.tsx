import React from 'react'
import { Col, OverlayTrigger, ProgressBar, Row, Tooltip } from 'react-bootstrap'
import Percentage from '../../../percentage/Percentage';
import {IInfoDataCards} from '../../../../models/components/InfoDataCard'

interface IProp{
    data :IInfoDataCards[]
}

const InfoDataCards:React.FC<IProp> = ({data}) => {
    const show=()=>{
        return data.map((data,index) =>{
           return <Col md={5} lg={3} key={index} className="d-flex align-items-center justify-content-center min-h-17 my-1 py-1 px-3 ">
                        <Row className="text-md-start text-xs-start">
                        <p className=" m-0  font-color-bluish ">
                            {data.heading}
                            <OverlayTrigger
                                placement="auto"
                                delay={{ show: 100, hide: 50 }}
                                overlay={<Tooltip id="button-tooltip">
                                    <div className="fw-bold m-1">
                                        {
                                            data.overlayTarget.map((text,textIndex) =>{
                                                return <p className="my-1" key={textIndex}>{text}</p>
                                            })
                                        }
                                    </div>
                                </Tooltip>}
                            >
                                <i className=" bi bi-info-circle-fill mx-2"></i>
                            </OverlayTrigger>
                        </p>
                        <p className="fs-5 fw-bold m-0 ">{data.numberWithSimbol}</p>
                        {
                            data.percentage === undefined 
                            ? <></>
                            :<Percentage classes="fw-bold" number={data.percentage}/>

                        }
    
                        {
                            data.progresBar === undefined 
                            ? <></>
                            :data.progresBar.max === null 
                                ? <></>
                                :<Col>
                                    <ProgressBar 
                                    variant="secondary" 
                                    className="mb-2"
                                    label={`${data.progresBar.label}%`}
                                    min={0} 
                                    max={data.progresBar.max} 
                                    now={data.progresBar.now} 
                                    />
                                </Col>
                        }
                        </Row>
                    </Col>
        })
    }

    return <>{show()}</>
    
}

export default InfoDataCards
