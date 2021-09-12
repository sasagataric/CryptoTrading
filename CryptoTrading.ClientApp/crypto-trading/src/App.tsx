import React , {useEffect,useState} from 'react';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'react-notifications/lib/notifications.css';
// @ts-ignore  
import { NotificationContainer } from 'react-notifications';
import { Route, Switch, Redirect } from 'react-router-dom';
import {Container, Spinner} from 'react-bootstrap'
//Components
import Header from "./components/header/Header"
import Cryptocurrencies from "./components/pages/cryptocurrencies/Cryptocurrencies"
import Portfolio from "./components/pages/portfolio/Portfolio"
import Watchlist from "./components/pages/watchlist/Watchlist"
import Wallet from "./components/pages/wallet/Wallet"
import CoinDetails from "./components/pages/coin-details/CoinDetails"
import PrivateRoute from './PrivateRoute'

import { walletService } from './services/walletService';
import { useDispatch } from 'react-redux';
import { setWallet } from './redux/actions/walletActions';

function App() {
  const [loginLoading,setLoginLoading] = useState<boolean>(false);

  const dispatch = useDispatch();
  useEffect(() => {
    (async()=>{
      const wallet = await walletService.getWalletById();
        if(wallet){
            dispatch(setWallet(wallet));
        }
    })()
  }, [dispatch])
 
  
  return (
    <div className="text-center bg-light min-vh-100 rel">
      <Header setLoginLoading={setLoginLoading}/>
      <Container fluid className="pt-4">
        {
          loginLoading &&
          <Spinner animation="border" className="mb-5 spinner-color" />
        }
      <Switch>
        <Route exact path="/" component={Cryptocurrencies} />
        <PrivateRoute path="/watchlist" component={Watchlist} />
        <PrivateRoute path="/portfolio" component={Portfolio} /> 
        <PrivateRoute path="/wallet" component={Wallet} /> 
        <Route path="/coin" component={CoinDetails} /> 
        <Redirect to="/" />
      </Switch>
      <NotificationContainer />
      </Container>

    </div>
  );
}

export default App;
