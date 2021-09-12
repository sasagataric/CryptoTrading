function getLocalAccessToken() {
    const accessToken = localStorage.getItem("access_token");
    return accessToken;
  }
  
function getLocalRefreshToken() {
    const refreshToken = localStorage.getItem("refresh_token");
    return refreshToken;
}

function setLocalAccessToken(access_token:string) {
    localStorage.setItem('access_token', "Bearer "+access_token);
}
  
function setLocalRefreshToken(refresh_token :string) {
    localStorage.setItem('refresh_token', refresh_token);
}

export const TokenService = {
    getLocalAccessToken,
    getLocalRefreshToken,
    setLocalAccessToken,
    setLocalRefreshToken
};