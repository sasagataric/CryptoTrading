import {IdentityServer} from './axios';
import {IIdentityServerResponse, IRefreshTokenResponse} from '../models/IdentityServerResponseModels';
// @ts-ignore  
import { NotificationManager } from "react-notifications";

export const accountService = {
    login,
    getNewAccesToken
};

async function login(accessToken: string)
{
    const params = new URLSearchParams()
    params.append('client_id', 'crypto-trading-web-app')
    params.append('client_secret', `${process.env.REACT_APP_CLIENT_SECRET}`)
    params.append('grant_type', 'external')
    params.append('scopes', 'WebApi.ExternalLogin')
    params.append('provider', 'google')
    params.append('external_token', accessToken)
    const config = {
        headers: {
          'Content-Type': 'application/x-www-form-urlencoded'
        }
      }
    return await IdentityServer.post<IIdentityServerResponse>("/connect/token", params, config)
                .then((res)=> {
                    localStorage.setItem('access_token', "Bearer "+res.data.access_token);
                    localStorage.setItem('refresh_token', res.data.refresh_token);
                    window.location.reload();
                })
                .catch(error => {
                    if (error.response) {
                        NotificationManager.error(error.response.data.errorMessage ,'Error',  1500);
                        return null;
                    } else if (error.request) {
                        NotificationManager.error('An error occurred while connecting to the server' ,'Error',  1500);
                        return null;
                    } else {
                        NotificationManager.error('Something went wrong' ,'Error',  1500);
                        return null;
                    }
                });
}

async function getNewAccesToken()
{
    let refresh_token = localStorage.getItem('refresh_token');
    if(!refresh_token) {
        NotificationManager.error('No refrash token');
        return null;
    }
    
    const params = new URLSearchParams()
    params.append('client_id', 'crypto-trading-web-app')
    params.append('client_secret', 'fb9060c08c20b4fd92c29731dfb8eeac6a34fcf6e72b6f48aad42ab7353e117b')
    params.append('grant_type', 'refresh_token')
    params.append('refresh_token', refresh_token)
    const config = {
        headers: {
          'Content-Type': 'application/x-www-form-urlencoded'
        }
      }
    return await IdentityServer.post<IRefreshTokenResponse>("/connect/token", params, config)
                .then((res)=> {
                    localStorage.setItem('access_token', "Bearer "+res.data.access_token);
                    localStorage.setItem('refresh_token', res.data.refresh_token);
                    window.location.reload();
                })
                .catch(error => {
                    if (error.response) {
                        NotificationManager.error(error.response.data.errorMessage ,'Error',  1500);
                        return null;
                    } else if (error.request) {
                        NotificationManager.error('An error occurred while connecting to the server' ,'Error',  1500);
                        return null;
                    } else {
                        NotificationManager.error('Something went wrong' ,'Error',  1500);
                        return null;
                    }
                });
}