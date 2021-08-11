import React from 'react';
import { GoogleLogin } from 'react-google-login';
import {loginService} from '../../services/loginService';

const responseGoogle = async (response:any ) => {
  console.log(JSON.stringify(response.mc.access_token));

  await loginService.login(response.tokenId);
}

function GoogleLoginButton() {
  return (
      <GoogleLogin
        clientId=""
        buttonText="Login"
        onSuccess={responseGoogle}
        onFailure={responseGoogle}
        cookiePolicy={'single_host_origin'}
      />
  );
}

export default GoogleLoginButton;
