/// <reference path="../components/login-form.js" />
/*eslint unicode-bom: ["error", "always"]*/

export const authenticationService = {
    login,
    logout, 
};


 async function login(username, password) {
    let returnStatus = 200;

    const requestOptions = {
        method: 'post',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ username, password })
    };

     await fetch('/account/Login', requestOptions)

        .then((callReturn) => {
            returnStatus = callReturn.status;
            return callReturn.ok ? callReturn.json() : null;
        })
        .then((json) => {
            const user = json;

            if (user != null) {
                sessionStorage.setItem('user', JSON.stringify(user));
            }
        })
        .catch((error) => {
            console.log('Error', error);
        });

    return returnStatus;
}

async function logout() {
    // remove user from  storage to log user out

    sessionStorage.clear();

    const requestOptions = {
        method: 'get',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    };

    //api controller - account/logout
    await fetch('/account/Logout', requestOptions)
 
        .catch (function (error) {
            console.log('Error', error);
        });
  

    window.location.href = '/Login';
}

