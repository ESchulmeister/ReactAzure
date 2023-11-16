import { withNavigationWatcher } from './contexts/navigation';

//class vn - BlobList
//import { DxQuestion } from './components/DxQuestion';


//functional components
import { default as Home } from './components/Home';
import { default as Login } from './components/login-form';
import { default as Upload } from './components/Upload';
import { default as PageNotFound } from './components/util/PageNotFound';
import { default as ProfileModal } from './components/ProfileModal';
import { default  as BlobList } from './components/BlobList';


const AppRoutes = [


    {
        path: '/Login',
        key: 'Login',
        element: <Login />
    },

    {
        path: '/Home',
        index:true,
        key: 'Home',
        element: <Home />
    },


    {
        path: '/Upload',
        key: 'Upload',
        element: <Upload />
    },


    {
        path: '/BlobList',
        key: 'BlobList',
        element: <BlobList />
    },

    {
        path: '/PageNotFound',
        key: 'PageNotFound',
        element: <PageNotFound />
    },

    {
        path: '/ProfileModal',
        key: 'ProfileModal',
        element: <ProfileModal />
    },

];


export default AppRoutes.map(route => {
    return {
        ...route,
        component: withNavigationWatcher(route.component)
    };
});
