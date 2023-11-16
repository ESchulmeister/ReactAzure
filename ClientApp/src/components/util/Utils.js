/*eslint unicode-bom: ["error", "always"]*/
const APP_ERROR = "An Unexpected  error has occurred.Please contact the system administrator.";

export const appError = () => {
    return APP_ERROR;
}



export const getUser = () => {
    return JSON.parse(sessionStorage.getItem('user'));
}


export const getAuthRequestOptions = () => {

    const user = getUser();
    const token = (user && user.Token) ? user.Token : '';

    const requestOptions = {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Authorization': token
        },
    };

    return requestOptions;
}


export const getAuthPostOptions = (state, body) => {

    let token = (state === null || state.auth === undefined ||
         state.auth === null || state.auth.headers === null || state.auth.headers === undefined) ? null : state.auth.headers.Authorization;
    if (token === undefined || token === null) {
        return null;
    }

    const requestOptions = {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json; charset=utf-8',
            'Authorization': token
        },
        body: JSON.stringify(body)
    };

    return requestOptions;

}


export const getDefaultReqOptions = () => {

    const requestOptions = {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
        },
    };

    return requestOptions;

}




