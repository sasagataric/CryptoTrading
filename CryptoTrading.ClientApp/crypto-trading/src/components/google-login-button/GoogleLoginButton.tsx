import React, { useState } from 'react';
import { GoogleLogin, GoogleLogout  } from 'react-google-login';
import { useHistory } from 'react-router-dom';
import {accountService} from '../../services/accountService';



interface IProps{
  isLoggedIn: boolean;
  closeNav: () => void;
  setLoginLoading: React.Dispatch<React.SetStateAction<boolean>>;
}

const GoogleLoginButton : React.FC<IProps>= ({isLoggedIn,closeNav,setLoginLoading})=> {
  const [login, setlogin] = useState(false)
  const history = useHistory();
  const responseGoogle = async (response:any ) => {
    closeNav();
    setlogin(true);
    setLoginLoading(true);
    await accountService.login(response.accessToken);
    setLoginLoading(false);
    setlogin(false);
  }
  
  const logout = () => {
    localStorage.clear();
    closeNav();
    history.push("/");
    setTimeout(() => {
      window.location.reload();
    }, 100);
  }
 const Check = () =>{
      if (!isLoggedIn) {
        return <GoogleLogin
        disabled={login}
        clientId={process.env.REACT_APP_GOOGLE_ID as string}
        className="text-dark border rounded-pill google-img-radius shadow-lite"
        buttonText="Login"
        onSuccess={responseGoogle}
        cookiePolicy={'single_host_origin'}
        
      />;
      } else {
        return <GoogleLogout
        clientId={process.env.REACT_APP_GOOGLE_ID as string}
        className="text-dark border-sm rounded-pill google-img-radius shadow-lite"
        buttonText="Logout"
        onLogoutSuccess={logout}
        />;
    }
  }

  return(
      <div className="ms-md-2 my-2 my-md-0">
        <Check />
      </div>
    )
  
}

export default GoogleLoginButton;
