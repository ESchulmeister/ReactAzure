/*eslint unicode-bom: ["error", "always"]*/

export default function authHeader() {

    const user = JSON.parse(sessionStorage.getItem('user'));

    if (user && user.Token) {
        return { 'Authorization': user.Token };
    }
    return {};
}