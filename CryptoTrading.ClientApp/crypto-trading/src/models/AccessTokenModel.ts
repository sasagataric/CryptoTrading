export interface IAccessToken {
    nbf: number;
    exp: number;
    iss: string;
    aud: string;
    client_id: string;
    sub: string;
    auth_time: number;
    idp: string;
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress": string;
    "AspNet.Identity.SecurityStamp": string;
    role: string;
    preferred_username: string;
    name: string;
    email: string;
    email_verified: boolean;
    picture: string;
    first_name: string;
    last_name: string;
    wallet_id:string;
    jti: string;
    iat: number;
    scope: string[];
    amr: string[];
}