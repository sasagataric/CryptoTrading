import axios from 'axios';
import {APIUrlConfig,IdentityServerURLConfig, CoinGeckoURLConfig}  from '../appSettings';
import { IRefreshTokenResponse } from '../models/IdentityServerResponseModels';
import {TokenService} from './tokenService'

const IdentityServer = axios.create({
  baseURL: IdentityServerURLConfig.baseURL
});

const CoinGecko = axios.create({
  baseURL: CoinGeckoURLConfig.baseURL
});
const API = axios.create({
  baseURL: APIUrlConfig.baseURL
});

API.interceptors.request.use(
  (config) => {
    const access_token = localStorage.getItem('access_token');
    if (access_token) {
      config.headers.authorization = access_token;
    }
    return config;
  },
  (error) => Promise.reject(error),
);

API.interceptors.response.use(
  (res) => {
    return res;
  },
  async (err) => {
    const originalConfig = err.config;
    // Access Token was expired
    if (err.response.status === 401 && !originalConfig._retry) {
      console.log(err.response);
      originalConfig._retry = true;
      let refresh_token =TokenService.getLocalRefreshToken();
      if(!refresh_token) {
        return Promise.reject(err);
      }
      
      try {
        const params = new URLSearchParams()
        params.append('client_id', 'crypto-trading-web-app')
        params.append('client_secret', `${process.env.REACT_APP_CLIENT_SECRET}`)
        params.append('grant_type', 'refresh_token')
        params.append('refresh_token', refresh_token)
        const config = {
            headers: {
              'Content-Type': 'application/x-www-form-urlencoded'
            }
        }
        const rs = await IdentityServer.post<IRefreshTokenResponse>("/connect/token", params, config);
        console.log(rs);
        TokenService.setLocalAccessToken(rs.data.access_token);
        TokenService.setLocalRefreshToken(rs.data.refresh_token);

        return API(originalConfig);
      } catch (_error) {
        return Promise.reject(_error);
      }
    }
    
    return Promise.reject(err);
  }
);



export {
  API,
  IdentityServer,
  CoinGecko
};