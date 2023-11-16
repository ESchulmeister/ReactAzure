/*eslint unicode-bom: ["error", "always"]*/

import React, { useEffect } from 'react';
import { withRouter } from './with-router';

const parseJwt = (token) => {
    try {
        return JSON.parse(atob(token.split('.')[1]));
    } catch (e) {
        return null;
    }
};



const AuthVerify = (props) => {

    useEffect(() => {
        const LoginRoute = 'Login';

        const user = JSON.parse(sessionStorage.getItem('user'));

        //redirect to login  if user is not set
        if (user === undefined || user === null) {
            if (window.location.href.indexOf(LoginRoute) < 0) {
                window.location.href = window.location.origin + '/' + LoginRoute;
            }
            return;
        }
        const decodedJwt = parseJwt(user.Token);

        if (decodedJwt.exp * 1000 < Date.now()) {
            props.logOut();
        }
    });

    return <div></div>;
};

export default withRouter(AuthVerify);
