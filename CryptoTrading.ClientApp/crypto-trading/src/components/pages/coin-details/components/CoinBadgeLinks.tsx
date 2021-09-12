import React from 'react'
import {Badge} from 'react-bootstrap'

interface IProps{
 hompage?:string |undefined,
 github?:string |undefined,
 reddit?:string |undefined,
 forum?:string |undefined,
}
const CoinBadgeLinks : React.FC<IProps> = (props) => {
    return (
        <>
            {
                props.hompage &&
                <a href={props.hompage} rel="noreferrer" target="_blank" className="col-auto">
                    <Badge 
                    bg="secondary" 
                    text="dark"  
                    className="font-09 bg-silver mx-1 mb-1 link">
                    <i className="bi bi-link-45deg "></i>  {props.hompage.split("//")[1]} 
                    </Badge>{' '}
                </a>
            }
            {
                props.github &&
                <a href={props.github} rel="noreferrer" target="_blank" className="col-auto">
                    <Badge 
                    bg="secondary" 
                    text="dark"  
                    className="font-09 bg-silver mx-1 mb-1 link">
                    <i className=" bi bi-code-slash"></i> Sorce code 
                    </Badge>{' '}
                </a>
            }
            {
                props.reddit &&
                <a href={props.reddit} rel="noreferrer" target="_blank" className="col-auto">
                    <Badge 
                    bg="secondary" 
                    text="dark"  
                    className="font-09 bg-silver mx-1 mb-1 link">
                    <i className="bi bi-reddit"></i> {props.reddit?.split("www.")[1]}
                    </Badge>{' '}
                </a>
            }        
            {
                props.forum &&
                <a href={props.forum} rel="noreferrer" target="_blank" className="col-auto">
                    <Badge 
                    bg="secondary" 
                    text="dark"  
                    className="font-09 bg-silver mx-1 mb-1 link">
                    <i className="bi bi-people-fill"></i> Forum
                    </Badge>{' '}
                </a>
            }        
        </>
    )
}

export default CoinBadgeLinks
