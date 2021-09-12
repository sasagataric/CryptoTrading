import React from 'react'

interface IProps {
    number: number |undefined;
    classes?: string
}
const Percentage:React.FC<IProps> = ({number,classes}) => {
    return (number===undefined? <p></p>
        :number===0 ? <p className={"mb-1 "+classes}>0%</p> 
            :number>0
            ?<p className={"mb-1 text-success "+classes}><i className="font-09 bi bi-caret-up-fill"></i>{number.toFixed(2)}%</p> 
            :<p className={"mb-1 text-danger "+classes}><i className="font-09 bi bi-caret-down-fill"></i>{number.toFixed(2).slice(1)}%</p>
       
    )
}

export default Percentage
