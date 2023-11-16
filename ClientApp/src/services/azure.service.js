/*eslint unicode-bom: ["error", "always"]*/
import { createStore } from 'devextreme-aspnet-data-nojquery';
import authHeader from '../services/auth-header';

export const azureService = {
    listBlobs, 
    uploadFile,
};

async function uploadFile(file) {
    let formData = new FormData();

  // formData.append( 'newName',Name);
    formData.append('file', file);

    const requestOptions = {
        method: 'post',
        body: formData, 
        headers: authHeader(),
    };

    return await fetch("azure/UploadFile", requestOptions);
};

 async function listBlobs() {
    return  new createStore({
        key: 'ID',
        loadUrl: 'azure/ListBlobs',

        onBeforeSend: (e, ajaxOptions) => {
            ajaxOptions.xhrFields = { withCredentials: true };
            ajaxOptions.headers = authHeader();
        },
    });

}
