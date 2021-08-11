import API from './axios';

export const loginService = {
    login
};

async function login(accessToken: string)
{
    
    return await API.post(`/api/Authentication/login`,{accessToken:accessToken})
                .then( (res)=> {
                    console.log(res.data);
                })
                .catch(error => {
                    console.log(error);
                    });

}