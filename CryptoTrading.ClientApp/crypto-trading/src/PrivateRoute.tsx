import React, { useEffect } from 'react'
import { Redirect, Route } from 'react-router-dom'
import {utils} from './utils/Utils'


const PrivateRoute : React.FC<{
    component: React.FC;
    path: string;
}> = (props) => {
    useEffect(() => {
        utils.isLoggedInWithNotification()
    },[]);

    return (
        utils.isLoggedIn() 
        ?(<Route path={props.path} component={props.component} />) 
        : (<Redirect  to={{ pathname: "/" } } />)
    )
}

export default PrivateRoute
