import React, {useState, useEffect} from 'react'
import { NavLink } from "react-router-dom";
import { Nav, Navbar , Container} from 'react-bootstrap'
import GoogleLoginButton from "../google-login-button/GoogleLoginButton"
import {utils} from '../../utils/Utils'
import jwt_decode from "jwt-decode";
import {IAccessToken} from "../../models/AccessTokenModel"

interface IProps{
    setLoginLoading: React.Dispatch<React.SetStateAction<boolean>>
}
const Header: React.FC<IProps> = ({setLoginLoading}) =>{

    const [isLoggedIn,setIsLoggedIn]=useState<boolean>(false);
    const [user,setUserData]=useState<IAccessToken>();
    const [expanded, setExpanded] = useState(false);

    const setNavExpanded =(expanded:boolean)=> {
        setExpanded(expanded);
      }
    
     const closeNav =()=> {
        setExpanded(false);
      }
    useEffect(()=>{
        let token = localStorage.getItem('access_token');
        if(token){
            const decodedToken = jwt_decode<IAccessToken>(token);
            setUserData(decodedToken);

           setIsLoggedIn(true);
        }else{
            setIsLoggedIn(false);
        }
        
    },[])
    const handleClick = (e:any) => {
        if(!utils.isLoggedInWithNotification()){
            e.preventDefault()
        }
    }

    return(
        <Container  fluid className="p-0 bg-white">
            <Container>
                <Navbar   
                variant="light" 
                sticky="top" 
                expand="md" 
                collapseOnSelect 
                onToggle={(e)=>setNavExpanded(e)}
                expanded={expanded}>
                    <Navbar.Brand  className="mx-3 text-dark">
                        <NavLink 
                        className="text-dark text-decoration-none "
                        to="/"
                        >CryptoTrading
                        </NavLink>
                    </Navbar.Brand>

                    <Navbar.Toggle className="shadow-none"/>
                    <Navbar.Collapse className="justify-content-end">
                    <Nav>
                        
                        <NavLink 
                        activeClassName="font-color-bluish fw-bold" 
                        className="text-dark text-decoration-none mx-2 my-2 my-md-0" 
                        to="/"
                        exact={true}
                        onClick={()=>closeNav()}
                        >Cryptocurrencies</NavLink>
                        <NavLink 
                        activeClassName="font-color-bluish fw-bold" 
                        className="text-dark text-decoration-none mx-2 my-2 my-md-0"  
                        to="/watchlist"
                        onClick={(e)=>{handleClick(e);closeNav();}}
                        >Watchlist</NavLink>
                        <NavLink 
                        activeClassName="font-color-bluish fw-bold" 
                        className="text-dark text-decoration-none mx-2 my-2 my-md-0" 
                        to="/portfolio"
                        onClick={(e)=>{handleClick(e);closeNav();}}
                        >Portfolio</NavLink>
                        <NavLink 
                        activeClassName="font-color-bluish fw-bold" 
                        className="text-dark text-decoration-none mx-2 my-2 my-md-0" 
                        to="/wallet"
                        onClick={(e)=>{handleClick(e);closeNav();}}
                        >Wallet</NavLink>
                    </Nav>
                    {
                        user?.picture && user?.last_name  &&
                        <>
                        <div className="d-flex justify-content-center align-items-center">
                        <Navbar.Text className="text-dark ms-md-4 ms-0 my-sm-2 my-md-0">
                            <img
                            src={user?.picture}
                            width="35"
                            height="35"
                            className="d-inline-block align-top rounded-circle shadow-lite border-dark"
                            alt="img"
                            />
                        </Navbar.Text>
                        
                        <Navbar.Text className="text-dark  fw-bold   mx-2 my-2 my-md-0">
                        {user.last_name}
                        </Navbar.Text>
                        </div>
                        </>
                    }    
                    <GoogleLoginButton 
                    setLoginLoading={setLoginLoading} 
                    closeNav={closeNav} 
                    isLoggedIn={isLoggedIn}/>               
                    </Navbar.Collapse>
                </Navbar>
            </Container>
        </Container>
    )
}

export default Header;