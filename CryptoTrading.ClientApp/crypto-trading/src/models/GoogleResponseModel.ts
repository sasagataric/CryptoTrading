export interface GoogleLoginResponse {
    Aa:          string;
    mc:          Mc;
    ct:          CT;
    googleId:    string;
    tokenObj:    Mc;
    tokenId:     string;
    accessToken: string;
    profileObj:  ProfileObj;
}

export interface CT {
    LS: string;
    Ue: string;
    uU: string;
    qS: string;
    DJ: string;
    Mt: string;
}

export interface Mc {
    token_type:      string;
    access_token:    string;
    scope:           string;
    login_hint:      string;
    expires_in:      number;
    id_token:        string;
    session_state:   SessionState;
    first_issued_at: number;
    expires_at:      number;
    idpId:           string;
}

export interface SessionState {
    extraQueryParams: ExtraQueryParams;
}

export interface ExtraQueryParams {
    authuser: string;
}

export interface ProfileObj {
    googleId:   string;
    imageUrl:   string;
    email:      string;
    name:       string;
    givenName:  string;
    familyName: string;
}
